using DbUp;
using FluentValidation;
using Journal.Application.User.Commands;
using Journal.Application.User.Commands.Validations;
using Journal.Domain.Abstractions;
using Journal.Infrastructure.MessageBus;
using Journal.Infrastructure.Persistence;
using Journal.Infrastructure.Persistence.Context;
using Journal.Infrastructure.Persistence.Repositories;
using Journal.Infrastructure.Persistence.Services;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;

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
            services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
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

            var redisConnection = configuration.GetSection("Redis").Value;

            if (!string.IsNullOrWhiteSpace(redisConnection))
            {
                services.AddStackExchangeRedisCache(options =>
                {
                    options.Configuration = redisConnection;
                    options.InstanceName = "journal:";
                });
            }
            else
            {
                services.AddDistributedMemoryCache();
            }

            var jwtKey = configuration.GetSection("Jwt:Key").Value;

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("Jwt:Key is not configured. Set it via user-secrets or the Jwt__Key environment variable.");
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ClockSkew = TimeSpan.FromMinutes(1)
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddCustomLog(this IServiceCollection services, IConfiguration configuration)
        {
            var loggerConfiguration = new LoggerConfiguration()
                            .MinimumLevel.Information()
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