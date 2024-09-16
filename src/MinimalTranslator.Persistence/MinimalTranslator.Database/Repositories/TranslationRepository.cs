using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Abstractions.Events;
using MinimalTranslator.Database.Events;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database.Repositories;

public class TranslationRepository : ITranslationRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ICacheService _cache;
    private readonly IEventBus _eventBus;

    public TranslationRepository(ApplicationDbContext dbContext, ICacheService cache, IEventBus eventBus)
    {
        _dbContext = dbContext;
        _cache = cache;
        _eventBus = eventBus;
    }

    public async Task AddAsync(Translation translation, CancellationToken cancellationToken = default)
    {
        await _dbContext.Translations.AddAsync(translation, cancellationToken);
        await _eventBus.PublishAsync(new TranslationUpsertIntegrationEvent(Guid.NewGuid(), translation), cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(Guid id, string language, CancellationToken cancellationToken = default)
    {
        var languageAsDomain = new Language(language);
        return await _dbContext.Translations
            .AnyAsync(x => x.Id == id && x.LanguageTo == languageAsDomain, cancellationToken);

        // return await _cache.GetAsync<TranslationResponse>($"translation/{id}/{language}", cancellationToken) is not null;
    }
}