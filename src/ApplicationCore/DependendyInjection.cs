using System.Reflection;
using ApplicationCore.Infrastructure.Persistence;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PJENL.API.CleanArchitecture.ApplicationCore.Common.Abstractions.Data;
using PJENL.API.CleanArchitecture.ApplicationCore.Common.Behaviours;
using PJENL.API.CleanArchitecture.ApplicationCore.Infrastructure.Persistence;
using PJENL.Template.CQRS.ApplicationCore.Common.Abstractions.Caching;
using PJENL.Template.CQRS.ApplicationCore.Infrastructure.Caching;

namespace PJENL.API.CleanArchitecture.ApplicationCore;
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
        return services;
    }
}
