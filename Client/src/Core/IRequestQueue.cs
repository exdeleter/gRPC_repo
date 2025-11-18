namespace Core;

public interface IRequestQueue
{
    ValueTask EnqueueAsync(QueueDto dto);
    ValueTask<QueueDto> DequeueAsync(CancellationToken cancellationToken);
}