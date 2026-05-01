using DbUp;
using FluentValidation;
using Journal.Domain.Abstractions;
using Journal.Infrastructure.MessageBus;
using Journal.Infrastructure.Persistence;
using Journal.Infrastructure.Persistence.Context;
using Journal.Infrastructure.Persistence.Repositories;
using Journal.Infrastructure.Persistence.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using StackExchange.Redis;
using System.Reflection;

namespace Journal.CrossCuting.AppDependency
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<DbServerData>();

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IJournalRepository, JournalRepository>();
            services.AddTransient<IQualisRepository, QualisRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IJournalRepository, JournalRepository>();
            services.AddHealthChecks().AddCheck<MysqlDbHealthCheckService>("MySQL DB");
            services.AddDbContext<JournalContext>(options =>
            {
                var connectionString = configuration.GetSection("ConnectionString").Value;
                options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            });

            services.AddSingleton(typeof(IPublisher<>), typeof(Publisher<>));
            services.AddMediatR(config => config.RegisterServicesFromAssemblies(Assembly.Load("Journal.Application")));
            services.AddValidatorsFromAssembly(Assembly.Load("Journal.Application"));
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration.GetSection("Redis").Value));

            return services;
        }

        public static IServiceCollection AddCustomLog(this IServiceCollection services, IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                            .WriteTo.Console();

            var seqUrl = configuration.GetSection("Seq").Value;

            if (!string.IsNullOrEmpty(seqUrl))
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Seq(seqUrl);
            }

            loggerConfiguration = loggerConfiguration.WriteTo.MySQL(configuration.GetSection("ConnectionString").Value);

            Log.Logger = loggerConfiguration.CreateLogger();

            services.AddSerilog(Log.Logger, true);

            return services;
        }

        public static void ConfigureDatabase(IConfiguration configuration)
        {
            var connectionString = configuration.GetSection("ConnectionString").Value;

            EnsureDatabase.For.MySqlDatabase(connectionString);

            var upgrader = DeployChanges.To
                                        .MySqlDatabase(connectionString)
                                        .WithScriptsEmbeddedInAssembly(Assembly.Load("Journal.Infrastructure"))
                                        .LogToAutodetectedLog()
                                        .Build();

            upgrader.PerformUpgrade();
        }
    }
}