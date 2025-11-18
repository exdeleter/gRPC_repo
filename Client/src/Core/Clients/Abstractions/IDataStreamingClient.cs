namespace Core.Clients.Abstractions;

public interface IDataStreamingClient<TData>
{
    Task<int> GetRequestId(CancellationToken cancellationToken);
    
    IAsyncEnumerable<StreamPage<TData>> StreamAsync(
        int requestId,
        CancellationToken cancellationToken);
}

public sealed class StreamPage<T>
{
    public int RequestId { get; init; }
    public int TotalCount { get; init; }
    public int Page { get; init; }
    public int Count { get; init; }

    public IReadOnlyList<T> Data { get; init; } = Array.Empty<T>();
}