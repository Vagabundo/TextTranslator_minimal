using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MinimalTranslator.Application.Abstractions.Data;
using MinimalTranslator.Application.Abstractions.Events;
using MinimalTranslator.Application.Translations;

namespace MinimalTranslator.Database.Events;

internal sealed class TranslationEventProcessorJob(
    InMemoryTranslationMessageQueue queue,
    ICacheService _cacheService,
    ILogger<TranslationEventProcessorJob> logger
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (ITranslationEvent intengrationEvent in queue.Reader.ReadAllAsync(stoppingToken))
        {
            logger.LogInformation("Processing message {IntegrationEventId}", intengrationEvent.Id);
            var translation = intengrationEvent.Translation;

            // Procesar mensaje y modificar cache en un try/catch e intentarlo unas cuantas veces si falla.
            // Echar un ojo a algo parecido a una deadletter queue en channels
            await _cacheService.SetAsync(
                $"translation/{translation.Id}/{translation.LanguageTo!.Value}",
                new TranslationResponse(
                    translation.TranslatedText!.Value,
                    translation.LanguageFrom!.Value,
                    translation.TranslatedText!.Value,
                    translation.LanguageTo!.Value
                ));

            logger.LogInformation("Message {IntegrationEventId} processed", intengrationEvent.Id);
        }
    }
}
