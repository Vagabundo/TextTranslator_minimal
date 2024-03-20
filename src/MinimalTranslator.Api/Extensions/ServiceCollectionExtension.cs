using MinimalTranslator.Api.Config;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;

namespace MinimalTranslator.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAzureHttpServices(this IServiceCollection services, IConfiguration configuration)
    {
        try{

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
        catch (Exception ex)
        {
            throw new Exception("Azure Http Services could not be initialized", ex);
        }
    }

    public static IServiceCollection AddLanguageConfig(this IServiceCollection services, IConfiguration configuration)
    {
        LanguageConfig languageConfig = new () { TargetLanguage = configuration.GetValue<string>("Language") };
        services.AddSingleton(languageConfig);

        return services;
    }
}
