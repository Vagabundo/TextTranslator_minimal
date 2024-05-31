using MediatR;
using MinimalTranslator.Api.Data;
using MinimalTranslator.Api.Config;
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

            var command = new CreateTranslationCommand(request.Text!, languageConfig.TargetLanguage!);
            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
        });

        app.MapGet("/api/translation/{id}",
        async (string id,
            ILogger<WebApplication> logger,
            LanguageConfig languageConfig,
            ISender sender,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTranslationQuery(id, languageConfig.TargetLanguage!);
            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result.Value.TranslatedText) : result.ToProblemDetails();
        });
    }
}