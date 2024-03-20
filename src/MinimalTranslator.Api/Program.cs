using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Database;
using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Database.Repositories;
using MinimalTranslator.Api;
using MinimalTranslator.Api.Extensions;
using MinimalTranslator.Domain.Translation;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContextPool<InMemoryContext>(options => 
{
    options.UseInMemoryDatabase("InMemoryTranslationsDatabase");
});

// Application
builder.Services.AddScoped<ITranslationService, TranslationService>();
builder.Services.AddAzureHttpServices(builder.Configuration);

// Persistence
builder.Services.AddScoped<ITranslationRepository, TranslationRepository>();

// Language
builder.Services.AddLanguageConfig(builder.Configuration);

var app = builder.Build();

app.MapTranslationEndpoints();

app.Run();