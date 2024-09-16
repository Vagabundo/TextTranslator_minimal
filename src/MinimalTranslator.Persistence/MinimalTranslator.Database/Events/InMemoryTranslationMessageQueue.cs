using System.Threading.Channels;
using MinimalTranslator.Application.Abstractions.Events;

namespace MinimalTranslator.Database.Events;

internal sealed class InMemoryTranslationMessageQueue
{
    private readonly Channel<ITranslationEvent> _channel = Channel.CreateUnbounded<ITranslationEvent>();

    public ChannelWriter<ITranslationEvent> Writer => _channel.Writer;
    public ChannelReader<ITranslationEvent> Reader => _channel.Reader;
}
