using System.Security.Cryptography;
using System.Text;
using MinimalTranslator.Api.ApiData;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Persistance.Database;
using MinimalTranslator.Core.Domain;
using MinimalTranslator.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Application
builder.Services.AddScoped<ITranslationService, TranslationService>();

// When using HttpServices
builder.Services.AddHttpClient();
builder.Services.Configure<AzureHttpConfig>(builder.Configuration.GetSection("Azure:Http"));
builder.Services.AddScoped<ITextAnalyticService, AzureHttpTextAnalyticService>();
builder.Services.AddScoped<ITextTranslatorService, AzureHttpTextTranslatorService>();

// Persistence
builder.Services.AddScoped<ITranslationRepository, TranslationCacheRepository>();

var app = builder.Build();

app.MapPost("/",
    async (TranslationRequest request,
        ILogger<WebApplication> logger,
        ITextAnalyticService textAnalyticsService,
        ITextTranslatorService textTranslatorService,
        ITranslationService translationService) =>
    {
        if (string.IsNullOrEmpty(request.Text))
        {
            return Results.BadRequest("Text cannot be empty");
        }

        try
        {
            byte[] hashBytes = SHA256.Create()
                .ComputeHash(Encoding.UTF8.GetBytes(request.Text))
                .Take(16)
                .ToArray();

            Guid id = new Guid(hashBytes);
            var language = await textAnalyticsService.GetLanguage(request.Text);

            var trans = new Translation
                {
                    Id = id,
                    TranslatedText = language == "es" ? request.Text : await textTranslatorService.Translate(request.Text, language, "es")
                };

            await translationService.Add(trans);

            return Results.Ok(id);
        }
        catch (Exception ex)
        {
            logger.LogError($"Error translating {request.Text}: {ex.Message}");
            return Results.BadRequest();
        }
    });

app.MapGet("/{id}",
    async (string id, ILogger<WebApplication> logger, ITranslationService translationService) =>
    {
        try
        {
            var guid = new Guid(id);
            var translation = await translationService.Get(guid);

            return translation is null ? Results.BadRequest("Translation not found") : Results.Ok(translation.TranslatedText);
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

app.Run();