using MinimalTranslator.Application.Data.Azure;
using MinimalTranslator.Application.Extensions;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Domain;
using MinimalTranslator.Domain.Translations;
using MinimalTranslator.SharedKernel;
using Moq;

namespace MinimalTranslator.Core.Tests;

public class TranslationServiceTest
{
    private TranslationService _translationService;
    private Mock<ITranslationRepository> _translationRepositoryMock;
    private Mock<ITextAnalyticService> _textAnalyticsServiceMock;
    private Mock<ITextTranslatorService> _textTranslatorServiceMock;
    private const string DefaultText = "Hello world!";
    private const string DefaultTranslation = "¡Hola mundo!";

    [SetUp]
    public void Setup()
    {
        _translationRepositoryMock = new Mock<ITranslationRepository>();
        _textAnalyticsServiceMock = new Mock<ITextAnalyticService>();
        _textTranslatorServiceMock = new Mock<ITextTranslatorService>();
        _translationService = new TranslationService(_translationRepositoryMock.Object, _textAnalyticsServiceMock.Object, _textTranslatorServiceMock.Object);
    }

    [TearDown]
    public void Dispose()
    {
        _translationRepositoryMock.Reset();
        _textAnalyticsServiceMock.Reset();
        _textTranslatorServiceMock.Reset();
    }

    [Test]
    public async Task Add_WhenTextIsValidAndTranslationIsNotInDB_ReturnsIdAndAddsTranslationToDB()
    {
        var text = DefaultText;
        var targetLanguage = "es";
        var sourceLanguage = "en";
        var translation = new Translation
        {
            Id = text.GetHashedId(),
            OriginalText = text,
            LanguageFrom = sourceLanguage,
            LanguageTo = targetLanguage,
            TranslatedText = DefaultTranslation
        };

        _textAnalyticsServiceMock.Setup(x => x.GetLanguage(It.IsAny<string>())).ReturnsAsync(Result.Success(sourceLanguage));
        _textTranslatorServiceMock.Setup(x => x.Translate(text, sourceLanguage, targetLanguage)).ReturnsAsync(Result.Success(translation.TranslatedText));
        _translationRepositoryMock.Setup(x => x.Get(translation.Id, targetLanguage)).ReturnsAsync((Translation?)null);
        _translationRepositoryMock.Setup(x => x.Add(It.IsAny<Translation>())).ReturnsAsync(translation);

        // Act
        var result = await _translationService.Add(text, targetLanguage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(translation.Id));
        _textAnalyticsServiceMock.VerifyAll();
        _textTranslatorServiceMock.VerifyAll();
        _translationRepositoryMock.VerifyAll();
        _translationRepositoryMock.Verify(x => x.Add(It.Is<Translation>(t =>
                t.Id == translation.Id &&
                t.OriginalText == translation.OriginalText &&
                t.LanguageFrom == translation.LanguageFrom &&
                t.LanguageTo == translation.LanguageTo &&
                t.TranslatedText == translation.TranslatedText)
            ), Times.Once);
    }

    [Test]
    public async Task Add_WhenTextIsValidAndTranslationIsInDB_ReturnsId()
    {
        var text = DefaultText;
        var targetLanguage = "es";
        var sourceLanguage = "en";
        var translation = new Translation
        {
            Id = text.GetHashedId(),
            OriginalText = text,
            LanguageFrom = sourceLanguage,
            LanguageTo = targetLanguage,
            TranslatedText = DefaultTranslation
        };

        _translationRepositoryMock.Setup(x => x.Get(translation.Id, targetLanguage)).ReturnsAsync(translation);

        // Act
        var result = await _translationService.Add(text, targetLanguage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(translation.Id));
        _translationRepositoryMock.VerifyAll();
        _translationRepositoryMock.Verify(x => x.Add(It.IsAny<Translation>()), Times.Never);
        _textAnalyticsServiceMock.Verify(x => x.GetLanguage(It.IsAny<string>()), Times.Never);
        _textTranslatorServiceMock.Verify(x => x.Translate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Add_WhenTextIsEmpty_ReturnsFailed()
    {
        var text = "";
        var targetLanguage = "es";

        // Act
        var result = await _translationService.Add(text, targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(TranslationErrors.NoText));
    }

    [Test]
    public async Task Add_WhenTargetLanguageIsEmpty_ReturnsFailed()
    {
        var text = DefaultText;
        var targetLanguage = "";

        // Act
        var result = await _translationService.Add(text, targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(TranslationErrors.NoTargetLanguage));
    }

    [Test]
    public async Task Add_WhenGetLanguageReturnsError_ReturnsFailed()
    {
        var text = DefaultText;
        var targetLanguage = "es";
        var error = DetectedLanguageResponseErrors.ExternalFailure;

        _textAnalyticsServiceMock.Setup(x => x.GetLanguage(It.IsAny<string>())).ReturnsAsync(Result.Failure<string>(error));

        // Act
        var result = await _translationService.Add(text, targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
        _textAnalyticsServiceMock.VerifyAll();
    }

    [Test]
    public async Task Add_WhenGetTranslatedTextReturnsError_ReturnsFailed()
    {
        var text = DefaultText;
        var targetLanguage = "es";
        var sourceLanguage = "en";
        var translation = new Translation
        {
            Id = text.GetHashedId(),
            OriginalText = text,
            LanguageFrom = sourceLanguage,
            LanguageTo = targetLanguage,
            TranslatedText = DefaultTranslation
        };
        var error = TranslatedTextResponseErrors.ExternalFailure;

        _textAnalyticsServiceMock.Setup(x => x.GetLanguage(It.IsAny<string>())).ReturnsAsync(Result.Success(sourceLanguage));
        _textTranslatorServiceMock.Setup(x => x.Translate(text, sourceLanguage, targetLanguage)).ReturnsAsync(Result.Failure<string>(error));

        // Act
        var result = await _translationService.Add(text, targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
        _textAnalyticsServiceMock.VerifyAll();
        _textTranslatorServiceMock.VerifyAll();
    }
}