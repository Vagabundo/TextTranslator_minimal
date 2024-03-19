using System.Security.Cryptography;
using System.Text;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Domain;
using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Services;

public class TranslationService : ITranslationService
{
    private readonly ITranslationRepository _translationRepository;
    private readonly ITextAnalyticService _textAnalyticsService;
    private readonly ITextTranslatorService _textTranslatorService;

    public TranslationService (ITranslationRepository translationRepository, ITextAnalyticService textAnalyticsService, ITextTranslatorService textTranslatorService)
    {
        _translationRepository = translationRepository;
        _textAnalyticsService = textAnalyticsService;
        _textTranslatorService = textTranslatorService;
    }

    public async Task<Guid> Add(string text, string targetLanguage)
    {
        Guid id = await GetHashedId(text);
        var translationEntity = await Get(id, targetLanguage);

        if (translationEntity is null)
        {
            var language = await _textAnalyticsService.GetLanguage(text);
            await Add(new Translation
                {
                    Id = id,
                    LanguageFrom = language,
                    OriginalText = text,
                    LanguageTo = targetLanguage,
                    TranslatedText = language == targetLanguage ? text
                                    : await _textTranslatorService.Translate(text, language, targetLanguage)
                });
        }

        return id;
    }

    public async Task<Translation> Add(Translation translation)
    {
        return await Get(translation) ?? await _translationRepository.Add(translation);
    }

    public async Task<Translation?> Get(Translation translation)
    {
        return await Get(translation.Id, translation.LanguageTo);
    }

    public async Task<Translation?> Get(Guid id, string language)
    {
        return await _translationRepository.Get(id, language);
    }

    public async Task<Guid> GetHashedId(string text)
    {
        byte[] hashBytes = SHA256.Create()
            .ComputeHash(Encoding.UTF8.GetBytes(text))
            .Take(16)
            .ToArray();

        return new Guid(hashBytes);
    }
}
