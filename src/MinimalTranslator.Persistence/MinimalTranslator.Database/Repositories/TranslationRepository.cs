using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Database.Abstractions;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database.Repositories;

public class TranslationRepository : ITranslationRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TranslationRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddAsync(Translation translation, CancellationToken cancellationToken = default)
    {
        await _dbContext.Translations.AddAsync(translation, cancellationToken);
    }

    public async Task<bool> AlreadyExistsAsync(Guid id, string language, CancellationToken cancellationToken = default)
    {
        var languageAsDomain = new Language(language);

        return await _dbContext.Translations
            .AnyAsync(x => x.Id == id && x.LanguageTo == languageAsDomain, cancellationToken);
    }
}