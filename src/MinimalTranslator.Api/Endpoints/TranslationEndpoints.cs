using MinimalTranslator.Api.ApiData;
using MinimalTranslator.Api.Config;
using MinimalTranslator.Application.Interfaces;

namespace MinimalTranslator.Api;

public static class TranslationEndpoints
{
    public static void MapTranslationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/translation",
        async (TranslationRequest request,
            ILogger<WebApplication> logger,
            LanguageConfig languageConfig,
            ITranslationService translationService) =>
        {
            try
            {
                if (string.IsNullOrEmpty(request.Text))
                {
                    return Results.BadRequest("Text cannot be empty");
                }

                Guid id = await translationService.Add(request.Text, languageConfig.TargetLanguage);

                return Results.Ok(id);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error translating {request.Text}: {ex.Message}");
                return Results.BadRequest();
            }
        });

        app.MapGet("/api/translation/{id}",
        async (string id,
            ILogger<WebApplication> logger,
            ITranslationService translationService,
            LanguageConfig languageConfig) =>
        {
            try
            {
                var guid = new Guid(id);
                var translation = await translationService.Get(guid, languageConfig.TargetLanguage);

                return translation is null || translation.TranslatedText is null
                ? Results.BadRequest("Translation not found")
                : Results.Ok(translation.TranslatedText);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is FormatException || ex is OverflowException)
            {
                logger.LogError($"Error creating Guid from {id}: {ex.Message}");
                return Results.BadRequest("Invalid Id provided");
            }
            catch (Exception ex)
            {
                logger.LogError($"Error getting translation {id}: {ex.Message}");
                return Results.BadRequest();
            }
        });
    }
}