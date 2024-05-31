using MediatR;
using MinimalTranslator.Api.Config;
using MinimalTranslator.Api.Data;
using MinimalTranslator.Api.Extensions;
using MinimalTranslator.Application.Translations.Create;
using MinimalTranslator.Application.Translations.Get;

namespace MinimalTranslator.Api;

public static class TranslationEndpoints
{
    public static void MapTranslationEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/translation",
        async (TranslationRequest request,
            ILogger<WebApplication> logger,
            LanguageConfig languageConfig,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var targetLanguage = string.IsNullOrEmpty(request.TargetLanguage) ? languageConfig.TargetLanguage! : request.TargetLanguage;
            var command = new CreateTranslationCommand(request.Text!, targetLanguage);
            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapGet("/api/translation/{id}",
        async (string id,
            string language,
            ILogger<WebApplication> logger,
            LanguageConfig languageConfig,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var targetLanguage = string.IsNullOrEmpty(language) ? languageConfig.TargetLanguage! : language;
            var query = new GetTranslationQuery(id, targetLanguage);
            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value.TranslatedText) : result.ToProblemDetails();
        });
    }
}