namespace MinimalTranslator.Domain.Translations;

public interface ITranslationRepository
{
    Task AddAsync(Translation translation, CancellationToken cancellationToken = default);
    Task<bool> AlreadyExistsAsync(Guid id, string language, CancellationToken cancellationToken = default);
}
