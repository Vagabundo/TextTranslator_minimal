using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Database.Abstractions;
using MinimalTranslator.Database.Cache;
using MinimalTranslator.Database.Data;
using MinimalTranslator.Database.Repositories;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDatabase(configuration);
        services.AddRedisCache(configuration);

        return services;
    }

    internal static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddPosgreSqlDatabase(configuration);

        services.AddScoped<ITranslationRepository, TranslationRepository>();
        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

        return services;
    }

    internal static IServiceCollection AddPosgreSqlDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PosgreSqlDatabase") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddTransient<IDbConnectionFactory>(
            _ => new NpgsqlConnectionFactory(connectionString));

        services.AddDbContextPool<IApplicationDbContext, ApplicationDbContext>(options => {
            options.UseNpgsql(connectionString);
        });

        return services;
    }

    #region Alternative Databases
    internal static IServiceCollection AddSQLiteDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("SQLiteDatabase") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddTransient<IDbConnectionFactory>(
            _ => new SqliteConnectionFactory(connectionString));

        services.AddDbContextPool<IApplicationDbContext, ApplicationDbContext>(options => {
            options.UseSqlite(connectionString);
        });

        return services;
    }

    internal static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddDbContextPool<IApplicationDbContext, ApplicationDbContext>(options => 
        {
            options.UseInMemoryDatabase("InMemoryTranslationsDatabase");
        });

        return services;
    }
    #endregion

    internal static IServiceCollection AddRedisCache(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = configuration.GetConnectionString("Redis") ?? throw new ArgumentNullException(nameof(configuration));
        });

        services.AddTransient<ICacheService, RedisCacheService>();
        //services.AddScoped<ITranslationRepository, TranslationRedisRepository>();

        return services;
    }
}
