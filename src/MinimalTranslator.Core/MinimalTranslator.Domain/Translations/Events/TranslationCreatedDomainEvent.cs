using MinimalTranslator.Domain.Abstractions;

namespace MinimalTranslator.Domain.Translations.Events;

public sealed record TranslationCreatedDomainEvent(Guid UserId) : IDomainEvent;
