using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Database.Abstractions;
using MinimalTranslator.Database.Data;
using MinimalTranslator.Database.Repositories;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSQLiteDatabase(configuration);

        return services;
    }

    public static IServiceCollection AddSQLiteDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("InMemorySQLiteDatabase") ?? throw new ArgumentNullException(nameof(configuration));

        services.AddTransient<IDbConnectionFactory>(
            _ => new SqliteConnectionFactory(connectionString));

        services.AddDbContextPool<IDbContext, InMemoryContext>(options => {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<ITranslationRepository, TranslationRepository>();

        services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<InMemoryContext>());

        return services;
    }

    public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddDbContextPool<IDbContext, InMemoryContext>(options => 
        {
            options.UseInMemoryDatabase("InMemoryTranslationsDatabase");
        });

        services.AddScoped<ITranslationRepository, TranslationRepository>();

        return services;
    }

    public static IServiceCollection AddRedisDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(redisOptions =>
        {
            redisOptions.Configuration = configuration.GetConnectionString("Redis") ?? throw new ArgumentNullException(nameof(configuration));
        });

        //services.AddScoped<ITranslationRepository, TranslationRedisRepository>();

        return services;
    }
}
