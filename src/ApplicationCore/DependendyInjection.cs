﻿using System.Reflection;
using System.Text;
using ApplicationCore.Common.Abstractions.Caching;
using ApplicationCore.Common.Abstractions.Data;
using ApplicationCore.Common.Behaviours;
using ApplicationCore.Infrastructure.Caching;
using ApplicationCore.Infrastructure.Persistence;
using ApplicationCore.Infrastructure.Persistence.Context;
using ApplicationCore.Infrastructure.Quartz;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Quartz;

namespace ApplicationCore;
public static class DependencyInjection
{
    public static IServiceCollection AddApplicationCore(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            config.AddOpenBehavior(typeof(ValidationBehaviour<,>));
            config.AddOpenBehavior(typeof(QueryCachingPipeLineBehavior<,>));
            config.AddOpenBehavior(typeof(CacheInvalidationPipeLineBehavior<,>));
        });
        //services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("defaultConnection")));
        services.AddTransient<IDbConnectionFactory, SqlConnectionFactory>();
        services.AddMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();
        return services;
    }

    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddHttpContextAccessor()
            .AddAuthorization()
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!))
                };
            });

        return services;
    }

    public static IServiceCollection AddQuartz(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz();

        services.AddQuartzHostedService(options =>
        {
            options.WaitForJobsToComplete = true;
        });

        services.ConfigureOptions<LoggingBackgroundJobSetup>();

        return services;
    }
}
