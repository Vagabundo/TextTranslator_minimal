using MinimalTranslator.Api.Config;

namespace MinimalTranslator.Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLanguageConfig(this IServiceCollection services, IConfiguration configuration)
    {
        LanguageConfig languageConfig = new () { TargetLanguage = configuration.GetValue<string>("Language") };
        services.AddSingleton(languageConfig);

        return services;
    }
}
