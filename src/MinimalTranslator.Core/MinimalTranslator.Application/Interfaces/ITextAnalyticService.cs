using MinimalTranslator.SharedKernel;

namespace MinimalTranslator.Application.Interfaces;

public interface ITextAnalyticService
{
    Task<Result<string>> GetLanguage(string text);
}
