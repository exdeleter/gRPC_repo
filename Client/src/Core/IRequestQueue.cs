namespace Core;

public interface IRequestQueue
{
    ValueTask EnqueueAsync(int requestId);
    ValueTask<int> DequeueAsync(CancellationToken cancellationToken);
}