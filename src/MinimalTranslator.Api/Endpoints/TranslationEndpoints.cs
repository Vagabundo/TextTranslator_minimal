using MinimalTranslator.Api.Data;
using MinimalTranslator.Api.Config;
using MinimalTranslator.Api.Extensions;
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
            var result = await translationService.Add(request.Text, languageConfig.TargetLanguage);
            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapGet("/api/translation/{id}",
        async (string id,
            ILogger<WebApplication> logger,
            ITranslationService translationService,
            LanguageConfig languageConfig) =>
        {
            var result = await translationService.Get(id, languageConfig.TargetLanguage);
            return result.IsSuccess ? Results.Ok(result.Value.TranslatedText) : result.ToProblemDetails();
        });
    }
}