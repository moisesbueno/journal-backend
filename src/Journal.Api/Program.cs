using HealthChecks.UI.Client;
using Journal.Api.Jobs;
using Journal.CrossCuting.AppDependency;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Quartz;

namespace Journal.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = builder.Configuration;

        builder.Services.AddCustomLog(configuration);

        DependencyInjection.ConfigureDatabase(configuration);

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddInfra(builder.Configuration);

        //builder.Services.AddHostedService<JournalConsumer>();

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

        app.MapHealthChecks(
            "/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

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