using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Database;
using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Database.Repositories;
using MinimalTranslator.Api.Config;
using MinimalTranslator.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<InMemoryContext>(options => 
{
    options.UseInMemoryDatabase("InMemoryTranslationsDatabase");
});

// Application
builder.Services.AddScoped<ITranslationService, TranslationService>();

// HttpServices
AzureHttpConfig azureHttpConfig = builder.Configuration.GetSection("AzureTranslator:Http").Get<AzureHttpConfig>();
builder.Services.AddScoped<ITextTranslatorService>(serviceProvider =>
{
    return new AzureHttpTextTranslatorService(azureHttpConfig.Uri, azureHttpConfig.Region, azureHttpConfig.Key);
});
builder.Services.AddScoped<ITextAnalyticService>(serviceProvider =>
{
    return new AzureHttpTextAnalyticService(azureHttpConfig.Uri, azureHttpConfig.Region, azureHttpConfig.Key);
});

// Persistence
builder.Services.AddScoped<ITranslationRepository, TranslationRepository>();

// Language
LanguageConfig languageConfig = new () { TargetLanguage = builder.Configuration.GetValue<string>("Language") };
builder.Services.AddSingleton(languageConfig);

var app = builder.Build();

app.MapTranslationEndpoints();

app.Run();