using MediatR;
using Microsoft.EntityFrameworkCore;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Exceptions;
using MinimalTranslator.Database.Abstractions;
using MinimalTranslator.Domain.Abstractions;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database;

public sealed class InMemoryContext : DbContext, IUnitOfWork, IDbContext
{
    private readonly IPublisher _publisher;

    public InMemoryContext(DbContextOptions options, IPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    public DbSet<Translation> Translations { set; get; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(InMemoryContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try{
            var result = await base.SaveChangesAsync(cancellationToken);
            await PublishDomainEventAsync();

            return result;
        }
        catch(DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException("Concurrency exception was triggered", ex);
        }
    }

    private async Task PublishDomainEventAsync()
    {
        var domainEvents = ChangeTracker
        .Entries<Entity>()
        .Select(entry => entry.Entity)
        .SelectMany(entity =>
        {
            var domainEvents = entity.DomainEvents;
            entity.ClearDomainEvents();
            return domainEvents;
        }).ToList();

        foreach(var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent);
        }
    }
}