namespace MinimalTranslator.Application.Interfaces;

public interface ITextAnalyticService
{
    Task<string> GetLanguage(string text);
}
