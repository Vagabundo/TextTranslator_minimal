using MinimalTranslator.Application.Data.Azure;
using MinimalTranslator.Application.Extensions;
using MinimalTranslator.Application.Interfaces;
using MinimalTranslator.Application.Services;
using MinimalTranslator.Domain.Translation;
using MinimalTranslator.SharedKernel;
using Moq;

namespace MinimalTranslator.Core.Tests;

public class TranslationServiceTest
{
    private TranslationService _translationService;
    private Mock<ITranslationRepository> _translationRepositoryMock;
    private Mock<ITextAnalyticService> _textAnalyticsServiceMock;
    private Mock<ITextTranslatorService> _textTranslatorServiceMock;
    private Translation? _validTranslation;
    private string _validText;
    private string _validTranslatedText;
    private string _validId;
    private string _sourceLanguage;
    private string _targetLanguage;

    [SetUp]
    public void Setup()
    {
        _translationRepositoryMock = new Mock<ITranslationRepository>();
        _textAnalyticsServiceMock = new Mock<ITextAnalyticService>();
        _textTranslatorServiceMock = new Mock<ITextTranslatorService>();
        _translationService = new TranslationService(_translationRepositoryMock.Object, _textAnalyticsServiceMock.Object, _textTranslatorServiceMock.Object);

        InitializeValidData();
        InitializeValidTranslation();
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
        _textAnalyticsServiceMock.Setup(x => x.GetLanguage(It.IsAny<string>())).ReturnsAsync(Result.Success(_sourceLanguage));
        _textTranslatorServiceMock.Setup(x => x.Translate(_validText, _sourceLanguage, _targetLanguage)).ReturnsAsync(Result.Success(_validTranslation.TranslatedText));
        _translationRepositoryMock.Setup(x => x.Get(_validTranslation.Id, _targetLanguage)).ReturnsAsync((Translation?)null);
        _translationRepositoryMock.Setup(x => x.Add(It.IsAny<Translation>())).ReturnsAsync(_validTranslation);

        // Act
        var result = await _translationService.Add(_validText, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(_validTranslation.Id));
        _textAnalyticsServiceMock.VerifyAll();
        _textTranslatorServiceMock.VerifyAll();
        _translationRepositoryMock.VerifyAll();
        _translationRepositoryMock.Verify(x => x.Add(It.Is<Translation>(t =>
                t.Id == _validTranslation.Id &&
                t.OriginalText == _validTranslation.OriginalText &&
                t.LanguageFrom == _validTranslation.LanguageFrom &&
                t.LanguageTo == _validTranslation.LanguageTo &&
                t.TranslatedText == _validTranslation.TranslatedText)
            ), Times.Once);
    }

    [Test]
    public async Task Add_WhenTextIsValidAndTranslationIsInDB_ReturnsId()
    {
        _translationRepositoryMock.Setup(x => x.Get(_validTranslation.Id, _targetLanguage)).ReturnsAsync(_validTranslation);

        // Act
        var result = await _translationService.Add(_validText, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(_validTranslation.Id));
        _translationRepositoryMock.VerifyAll();
        _translationRepositoryMock.Verify(x => x.Add(It.IsAny<Translation>()), Times.Never);
        _textAnalyticsServiceMock.Verify(x => x.GetLanguage(It.IsAny<string>()), Times.Never);
        _textTranslatorServiceMock.Verify(x => x.Translate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Test]
    public async Task Add_WhenTextIsEmpty_ReturnsNoTextError()
    {
        var invalidText = "";

        // Act
        var result = await _translationService.Add(invalidText, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(TranslationErrors.NoText));
    }

    [Test]
    public async Task Add_WhenTargetLanguageIsEmpty_ReturnsNoTargetLanguageError()
    {
        var invalidTargetLanguage = "";

        // Act
        var result = await _translationService.Add(_validText, invalidTargetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(TranslationErrors.NoTargetLanguage));
    }

    [Test]
    public async Task Add_WhenLanguageServiceReturnsError_ReturnsExternalFailureError()
    {
        var error = DetectedLanguageResponseErrors.ExternalFailure;

        _textAnalyticsServiceMock.Setup(x => x.GetLanguage(It.IsAny<string>())).ReturnsAsync(Result.Failure<string>(error));

        // Act
        var result = await _translationService.Add(_validText, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
        _textAnalyticsServiceMock.VerifyAll();
    }

    [Test]
    public async Task Add_WhenTranslationServiceReturnsError_ReturnsExternalFailureError()
    {
        var error = TranslatedTextResponseErrors.ExternalFailure;

        _textAnalyticsServiceMock.Setup(x => x.GetLanguage(It.IsAny<string>())).ReturnsAsync(Result.Success(_sourceLanguage));
        _textTranslatorServiceMock.Setup(x => x.Translate(_validText, _sourceLanguage, _targetLanguage)).ReturnsAsync(Result.Failure<string>(error));

        // Act
        var result = await _translationService.Add(_validText, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
        _textAnalyticsServiceMock.VerifyAll();
        _textTranslatorServiceMock.VerifyAll();
    }

    [Test]
    public async Task Get_WhenIdIsEmpty_ReturnsInvalidIdError()
    {
        var emptyId = "";
        var error = TranslationErrors.InvalidId(emptyId);

        // Act
        var result = await _translationService.Get(emptyId, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
    }

    [Test]
    public async Task Get_WhenIdIsShort_ReturnsInvalidIdError()
    {
        var invalidId = "11f09a-5a60-b9a-022f-248fb794";
        var error = TranslationErrors.InvalidId(invalidId);
        
        // Act
        var result = await _translationService.Get(invalidId, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
    }

    [Test]
    public async Task Get_WhenIdIsInvalid_ReturnsInvalidIdError()
    {
        var invalidId = "11f00a9a75a605b-96a8022f-248fb797064a";
        var error = TranslationErrors.InvalidId(invalidId);
        
        // Act
        var result = await _translationService.Get(invalidId, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
    }

    [Test]
    public async Task Get_WhenTranslationIsNotInDB_ReturnsNotFoundError()
    {
        var validGuid = new Guid(_validId);
        var error = TranslationErrors.NotFound(validGuid, _targetLanguage);
        
        _translationRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), _targetLanguage)).ReturnsAsync((Translation?)null);
        // Act
        var result = await _translationService.Get(_validId, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsFailure);
        Assert.That(result.Error, Is.EqualTo(error));
    }

    [Test]
    public async Task Get_WhenTranslationIsInDB_ReturnsTranslation()
    {
        InitializeValidTranslation();
        
        _translationRepositoryMock.Setup(x => x.Get(It.IsAny<Guid>(), _targetLanguage)).ReturnsAsync(_validTranslation);
        // Act
        var result = await _translationService.Get(_validId, _targetLanguage);

        // Assert
        Assert.IsTrue(result.IsSuccess);
        Assert.That(result.Value, Is.EqualTo(_validTranslation));
    }

    private void InitializeValidData()
    {
        _validText = "test text";
        _validTranslatedText = "texto de test";
        _validId = "11f00a9a-5a60-b96a-022f-248fb797064b";
        _sourceLanguage = "en";
        _targetLanguage = "es";
    }
    private void InitializeValidTranslation()
    {
        _validTranslation = new Translation
        {
            Id = _validText.GetHashedId(),
            OriginalText = _validText,
            LanguageFrom = _sourceLanguage,
            LanguageTo = _targetLanguage,
            TranslatedText = _validTranslatedText
        };
    }
}