using MinimalTranslator.Application.Extensions;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Domain.Translation;
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

    public async Task<Result<Guid>> Add(string? text, string? targetLanguage)
    {
        if (string.IsNullOrEmpty(text))
        {
            return Result.Failure<Guid>(TranslationErrors.NoText);
        }

        if (string.IsNullOrEmpty(targetLanguage))
        {
            return Result.Failure<Guid>(TranslationErrors.NoTargetLanguage);
        }

        Guid id = text.GetHashedId();
        var translationEntityResult = await Get(id, targetLanguage);

        if (translationEntityResult is not null && translationEntityResult.IsFailure)
        {
            var languageResult = await _textAnalyticsService.GetLanguage(text);
            if (languageResult.IsFailure)
            {
                return Result.Failure<Guid>(languageResult.Error);
            }

            var language = languageResult.Value;
            var translatedTextResult = await GetTranslatedText(text, language, targetLanguage);
            if (translatedTextResult.IsFailure)
            {
                return Result.Failure<Guid>(translatedTextResult.Error);
            }

            await _translationRepository.Add(new Translation
                {
                    Id = id,
                    LanguageFrom = language,
                    OriginalText = text,
                    LanguageTo = targetLanguage,
                    TranslatedText = translatedTextResult.Value
                });
        }

        return id;
    }

    public async Task<Result<Translation>> Get(string id, string? language)
    {
        return Guid.TryParse(id, out var guid) ?
            await Get(guid, language) :
            Result.Failure<Translation>(TranslationErrors.InvalidId(id));
    }

    private async Task<Result<Translation>> Get(Guid id, string? language)
    {
        if (string.IsNullOrEmpty(language))
        {
            return Result.Failure<Translation>(TranslationErrors.NoTargetLanguage);
        }

        var entity = await _translationRepository.Get(id, language);

        return entity is null ? Result.Failure<Translation>(TranslationErrors.NotFound(id, language)) : entity;
    }

    private async Task<Result<string>> GetTranslatedText(string text, string currentLanguage, string targetLanguage)
    {
        if (currentLanguage == targetLanguage)
        {
            return text;
        }

        var translatedTextResult = await _textTranslatorService.Translate(text, currentLanguage, targetLanguage);
        return translatedTextResult.IsSuccess ? translatedTextResult.Value : Result.Failure<string>(translatedTextResult.Error);
    }
}