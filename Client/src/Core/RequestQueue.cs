namespace Core;

using System.Threading.Channels;

public class RequestQueue : IRequestQueue
{
    private readonly Channel<int> _queue;

    public RequestQueue()
    {
        _queue = Channel.CreateUnbounded<int>();
    }

    public async ValueTask EnqueueAsync(int requestId)
    {
        await _queue.Writer.WriteAsync(requestId);
    }

    public async ValueTask<int> DequeueAsync(CancellationToken cancellationToken)
    {
        return await _queue.Reader.ReadAsync(cancellationToken);
    }
}