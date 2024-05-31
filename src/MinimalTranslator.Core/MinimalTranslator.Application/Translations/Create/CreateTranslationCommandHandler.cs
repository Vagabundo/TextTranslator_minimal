using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Abstractions.Messaging;
using MinimalTranslator.Application.Abstractions.Services;
using MinimalTranslator.Application.Extensions;
using MinimalTranslator.Domain.Translation;
using MinimalTranslator.Domain.Translations;
using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Translations.Create;

internal sealed class CreateTranslationCommandHandler : ICommandHandler<CreateTranslationCommand, Guid>
{
    private readonly ITranslationRepository _translationRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITextAnalyticService _textAnalyticsService;
    private readonly ITextTranslatorService _textTranslatorService;


    public CreateTranslationCommandHandler(
        ITranslationRepository translationRepository, IUnitOfWork unitOfWork,
        ITextAnalyticService textAnalyticsService, ITextTranslatorService textTranslatorService)
    {
        _translationRepository = translationRepository;
        _unitOfWork = unitOfWork;
        _textAnalyticsService = textAnalyticsService;
        _textTranslatorService = textTranslatorService;
    }

    public async Task<Result<Guid>> Handle(CreateTranslationCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Text))
        {
            return Result.Failure<Guid>(TranslationErrors.NoText);
        }

        if (string.IsNullOrEmpty(request.TargetLanguage))
        {
            return Result.Failure<Guid>(TranslationErrors.NoTargetLanguage);
        }

        Guid id = request.Text.GetHashedId();

        if (!await _translationRepository.AlreadyExistsAsync(id, request.TargetLanguage))
        {
            var languageResult = await _textAnalyticsService.GetLanguage(request.Text);
            if (languageResult.IsFailure)
            {
                return Result.Failure<Guid>(languageResult.Error);
            }

            var language = languageResult.Value;
            var translatedTextResult = await TranslateText(request.Text, language, request.TargetLanguage);
            if (translatedTextResult.IsFailure)
            {
                return Result.Failure<Guid>(translatedTextResult.Error);
            }

            await _translationRepository.AddAsync(
                Translation.Create(
                    id, new Text(request.Text), new Language(language),
                    new Text(translatedTextResult.Value), new Language(request.TargetLanguage)),
                cancellationToken
            );

            await _unitOfWork.SaveChangesAsync();
        }

        return id;
    }

    private async Task<Result<string>> TranslateText(string text, string currentLanguage, string targetLanguage)
    {
        if (currentLanguage == targetLanguage)
        {
            return text;
        }

        var translatedTextResult = await _textTranslatorService.Translate(text, currentLanguage, targetLanguage);
        return translatedTextResult.IsSuccess ? translatedTextResult.Value : Result.Failure<string>(translatedTextResult.Error);
    }
}
