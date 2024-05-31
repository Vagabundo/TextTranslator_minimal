using MinimalTranslator.Database.Abstractions;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database.Repositories;

public class TranslationRepository : ITranslationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;

    public TranslationRepository(ApplicationDbContext dbContext, ICacheService cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }

    public async Task AddAsync(Translation translation, CancellationToken cancellationToken = default)
    {
        await _dbContext.Translations.AddAsync(translation, cancellationToken);
        await _cache.SetAsync($"translation/{translation.Id}/{translation.LanguageTo!.Value}", translation);
    }

    public async Task<bool> AlreadyExistsAsync(Guid id, string language, CancellationToken cancellationToken = default)
    {
        return await _cache.GetAsync<Translation>($"translation/{id}/{language}", cancellationToken) is not null;
    }
}