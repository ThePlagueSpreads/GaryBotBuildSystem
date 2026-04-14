using Core;

namespace GaryBotServer;

using System.Threading.Channels;

public sealed class SerialBuildQueue : IBuildQueue
{
    private readonly Channel<BuildRequest> _channel;

    public SerialBuildQueue()
    {
        _channel = Channel.CreateUnbounded<BuildRequest>(
            new UnboundedChannelOptions
            {
                SingleReader = true,
                SingleWriter = false
            });
    }

    public async Task EnqueueAsync(
        BuildRequest request,
        CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(request, ct);
    }

    public async IAsyncEnumerable<BuildRequest> ReadAllAsync(
        [System.Runtime.CompilerServices.EnumeratorCancellation]
        CancellationToken ct = default)
    {
        await foreach (var request in _channel.Reader.ReadAllAsync(ct))
        {
            yield return request;
        }
    }

    public bool Clear()
    {
        bool cleared = false;
        
        while (_channel.Reader.TryRead(out _))
        {
            cleared = true;
        }

        return cleared;
    }
}