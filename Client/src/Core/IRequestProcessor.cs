namespace Core;

public interface IRequestProcessor
{
    Task ProcessAsync(int requestId, CancellationToken token);
}