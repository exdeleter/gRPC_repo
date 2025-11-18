namespace Core;

public interface IRequestProcessor
{
    Task ProcessAsync(QueueDto dto, CancellationToken token);
}