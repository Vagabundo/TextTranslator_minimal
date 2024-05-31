using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Abstractions.Services;

public interface ITextAnalyticService
{
    Task<Result<string>> GetLanguage(string text);
}
