using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Translations;
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
        await _cache.SetAsync(
            $"translation/{translation.Id}/{translation.LanguageTo!.Value}",
            new TranslationResponse(
                translation.TranslatedText!.Value,
                translation.LanguageFrom!.Value,
                translation.TranslatedText!.Value,
                translation.LanguageTo!.Value
            ));
    }

    public async Task<bool> AlreadyExistsAsync(Guid id, string language, CancellationToken cancellationToken = default)
    {
        var languageAsDomain = new Language(language);
        return await _dbContext.Translations
            .AnyAsync(x => x.Id == id && x.LanguageTo == languageAsDomain, cancellationToken);

        // return await _cache.GetAsync<TranslationResponse>($"translation/{id}/{language}", cancellationToken) is not null;
    }
}