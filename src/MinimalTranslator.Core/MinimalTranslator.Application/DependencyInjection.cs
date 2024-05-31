using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MinimalTranslator.Application.Abstractions.Services;
using MinimalTranslator.Application.Extensions.Behaviors;
using MinimalTranslator.Application.Services.Azure;
using MinimalTranslator.Application.Services.Azure.Data.Config;

namespace MinimalTranslator.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAzureHttpServices(configuration);

        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        return services;
    }

    private static IServiceCollection AddAzureHttpServices(this IServiceCollection services, IConfiguration configuration)
    {
        AzureHttpConfig? azureHttpConfig = configuration.GetSection("AzureTranslator:Http").Get<AzureHttpConfig>();

        if (azureHttpConfig is null || azureHttpConfig.Uri is null || azureHttpConfig.Region is null ||
        azureHttpConfig.Key is null || azureHttpConfig.LanguageRecognitionScoreThreshold is 0)
        {
            throw new Exception("Azure translator http config invalid");
        }
        
        services.AddScoped<ITextTranslatorService>(serviceProvider =>
        {
            return new AzureHttpTextTranslatorService(azureHttpConfig.Uri, azureHttpConfig.Region, azureHttpConfig.Key);
        });
        
        services.AddScoped<ITextAnalyticService>(serviceProvider =>
        {
            return new AzureHttpTextAnalyticService(azureHttpConfig.Uri, azureHttpConfig.Region, azureHttpConfig.Key, azureHttpConfig.LanguageRecognitionScoreThreshold);
        });

        return services;
    }
}
