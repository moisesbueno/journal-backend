using FluentValidation;
using FluentValidation.AspNetCore;
using Journal.Api.Consumers;
using Journal.Api.Jobs;
using Journal.CrossCuting.AppDependency;
using Journal.Domain.Abstractions;
using Journal.Infrastructure.Persistence.Repositories;
using Quartz;
using Serilog;
using StackExchange.Redis;

namespace Journal.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            //.WriteTo.Seq(builder.Configuration.GetSection("Seq").Value)
            .WriteTo.MySQL(builder.Configuration.GetSection("ConnectionString").Value)
            .CreateLogger();

        builder.Services.AddSerilog();

        DependencyInjection.ConfigureDatabase(builder.Configuration);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(typeof(Program).Assembly);

        builder.Services.AddInfra(builder.Configuration);

        builder.Services.AddTransient<IJournalRepository, JournalRepository>();
        builder.Services.AddTransient<IQualisRepository, QualisRepository>();
        builder.Services.AddTransient<IUserRepository, UserRepository>();
        //builder.Services.AddHostedService<JournalConsumer>();
        builder.Services.AddSingleton<IConnectionMultiplexer>(
            ConnectionMultiplexer.Connect(builder.Configuration.GetSection("Redis").Value));

        builder.Services.AddQuartz(q =>
        {
            var jobkey = new JobKey(nameof(CleanLogJob));

            q.AddJob<CleanLogJob>(opts => opts.WithIdentity(jobkey));

            q.AddTrigger(opts => opts
                .ForJob(jobkey)
                .WithIdentity($"{jobkey}-trigger")
                .WithCronSchedule("0 0 */12 ? * *"));
        });

        builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}