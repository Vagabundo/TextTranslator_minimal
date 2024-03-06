using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Persistance.Database;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddStackExchangeRedisCache(redisOptions =>
{
    redisOptions.Configuration = builder.Configuration.GetConnectionString("Redis");
});

// Application
builder.Services.AddScoped<ITranslationService, TranslationService>();

// Persistence
builder.Services.AddScoped<ITranslationRepository, TranslationCacheRepository>();


var app = builder.Build();

app.MapPost("/", () => "Hello World!");

app.MapGet("/", () => "Hello World!");

app.Run();