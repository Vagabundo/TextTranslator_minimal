using MinimalTranslator.Application.Abstractions.Events;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database.Events;

public record TranslationUpsertIntegrationEvent(Guid Id, Translation Translation) : TranslationEvent(Id, Translation);