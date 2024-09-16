using MinimalTranslator.Application.Abstractions.Events;

namespace MinimalTranslator.Database.Events;

internal sealed class EventBus(InMemoryTranslationMessageQueue queue) : IEventBus
{
    public async Task PublishAsync<T>(T translationEvent, CancellationToken cancellationToken = default)
        where T : class, ITranslationEvent
    {
        await queue.Writer.WriteAsync(translationEvent, cancellationToken);
    }
}