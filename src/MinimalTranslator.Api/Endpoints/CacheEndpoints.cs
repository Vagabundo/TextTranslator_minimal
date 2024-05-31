using MinimalTranslator.Database.Abstractions;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Api;

public static class CacheEndpoints
{
    public static void MapCacheEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/translation/populatecache",
        async (IApplicationDbContext dbContext,
            ICacheService cache) =>
        {
            foreach(Translation translation in dbContext.Translations)
            {
                await cache.SetAsync($"translation/{translation.Id}/{translation.LanguageTo!.Value}", translation);
            }

            return Results.Ok();
        });
    }
}