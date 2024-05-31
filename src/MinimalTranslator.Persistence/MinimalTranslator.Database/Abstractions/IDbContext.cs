using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database.Abstractions;

public interface IDbContext
{
    DbSet<Translation> Translations { set; get; }
    // DatabaseFacade Database { get; }
}
