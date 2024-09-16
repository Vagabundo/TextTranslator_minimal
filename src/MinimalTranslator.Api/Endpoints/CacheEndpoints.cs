using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Translations;
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
                await cache.SetAsync(
                    $"translation/{translation.Id}/{translation.LanguageTo!.Value}",
                    new TranslationResponse(
                        translation.TranslatedText!.Value,
                        translation.LanguageFrom!.Value,
                        translation.TranslatedText!.Value,
                        translation.LanguageTo!.Value
                    ));
            }

            return Results.Ok();
        });
    }
}