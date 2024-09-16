using MinimalTranslator.Application;
using MinimalTranslator.Database;
using MinimalTranslator.Api;
using MinimalTranslator.Api.Extensions;
using MinimalTranslator.Api.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Persistence
builder.Services.AddDatabaseServices(builder.Configuration);

// Application
builder.Services.AddApplicationServices(builder.Configuration);

// Config
builder.Services.AddLanguageConfig(builder.Configuration);

// Along with app.UseExceptionHandler()
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseMiddleware<ExceptionHandlingMiddleware>();
app.ApplyMigration();
app.UseExceptionHandler();

app.MapTranslationEndpoints();
app.MapCacheEndpoints();

app.Run();

// This is to allow integration tests in MinimalTranslatorApi.Tests project
public partial class Program { }