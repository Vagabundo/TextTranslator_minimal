using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Domain;

namespace MinimalTranslator.Database;

public class InMemoryContext : DbContext
{
    public InMemoryContext(DbContextOptions<InMemoryContext> options) : base(options) {}

    public DbSet<Translation> Translations { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Translation>()
                    .HasIndex(x => new { x.Id});
    }
}
