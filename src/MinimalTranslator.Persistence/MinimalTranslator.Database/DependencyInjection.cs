using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Domain.Translation;
using MinimalTranslator.Database.Repositories;

namespace MinimalTranslator.Database;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInMemoryDatabase();

        return services;
    }

    public static IServiceCollection AddInMemoryDatabase(this IServiceCollection services)
    {
        services.AddDbContextPool<InMemoryContext>(options => 
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
            redisOptions.Configuration = configuration.GetConnectionString("Redis");
        });

        //services.AddScoped<ITranslationRepository, TranslationRedisRepository>();

        return services;
    }
}
