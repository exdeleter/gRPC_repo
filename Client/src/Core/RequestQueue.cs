namespace Core;

using System.Threading.Channels;

public class RequestQueue : IRequestQueue
{
    private readonly Channel<QueueDto> _queue;

    public RequestQueue()
    {
        _queue = Channel.CreateUnbounded<QueueDto>();
    }

    public async ValueTask EnqueueAsync(QueueDto dto)
    {
        await _queue.Writer.WriteAsync(dto);
    }

    public async ValueTask<QueueDto> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}

public record QueueDto(int RequestId, int ExternalId);