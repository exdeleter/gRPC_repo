using Core;

using System.Runtime.CompilerServices;

using Core.Clients.Abstractions;

using Grpc.Core;

using Microsoft.Extensions.Logging;

public class GrpcDataStreaming : IDataStreamingClient<ResponseItem>
{
    private readonly DataProcessing.DataProcessingClient _client;
    // private readonly ILogger<GrpcDataStreaming> _logger;

    public GrpcDataStreaming(
        DataProcessing.DataProcessingClient client
        // ILogger<GrpcDataStreaming> logger
        )
    {
        _client = client;
        // _logger = logger;
    }

    public async Task<int> GetRequestId(CancellationToken cancellationToken)
    {
        var request = new EnqueueRequestDto { };

        var call = await _client.EnqueueRequestAsync(request, cancellationToken: cancellationToken);

        return call.RequestId;
    }

    public async IAsyncEnumerable<StreamPage<ResponseItem>> StreamAsync(
        int requestId,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var request = new StreamRequestDto
        {
            RequestId = requestId
        };

        using var call = _client.StreamData(request, cancellationToken: cancellationToken);

        IAsyncStreamReader<StreamResponseDto> responseStream = call.ResponseStream;

        await foreach (var response in responseStream.ReadAllAsync(cancellationToken))
        {
            yield return MapResponse(response);
        }
    }

    private StreamPage<ResponseItem> MapResponse(StreamResponseDto response)
    {
        return new StreamPage<ResponseItem>
        {
            RequestId = response.RequestId,
            TotalCount = response.TotalCount,
            Page = response.Page,
            Count = response.Count,
            Data = response.Data
                .Select(MapItem)
                .ToList()
        };
    }

    private ResponseItem MapItem(DataItem item)
    {
        return new ResponseItem
        {
            Key = item.Value,      // или item.Id — зависит от твоей схемы
            Value = item.Description
        };
    }
}

public sealed class ResponseItem
{
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
}