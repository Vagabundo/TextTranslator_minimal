using MinimalTranslator.Api.Config;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;

namespace MinimalTranslator.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureHttpServices(this IServiceCollection services, IConfiguration configuration)
    {
        AzureHttpConfig azureHttpConfig = configuration.GetSection("AzureTranslator:Http").Get<AzureHttpConfig>();
        
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

    public static IServiceCollection AddLanguageConfig(this IServiceCollection services, IConfiguration configuration)
    {
        LanguageConfig languageConfig = new () { TargetLanguage = configuration.GetValue<string>("Language") };
        services.AddSingleton(languageConfig);

        return services;
    }
}
