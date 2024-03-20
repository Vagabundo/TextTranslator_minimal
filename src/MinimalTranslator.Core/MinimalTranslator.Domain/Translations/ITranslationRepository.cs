namespace MinimalTranslator.Domain.Translation;

public interface ITranslationRepository
{
    Task<Translation> Add(Translation translation);
    Task<Translation?> Get(Guid id, string language);
}
