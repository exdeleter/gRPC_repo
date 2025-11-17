using Grpc.Core;

// namespace из proto

namespace Service.Services;

public class DataProcessingService : DataProcessing.DataProcessingBase
{
    private static readonly Dictionary<int, string> RequestStatuses = new();
    private readonly ILogger<DataProcessingService> _logger;

    public DataProcessingService(ILogger<DataProcessingService> logger)
    {
        _logger = logger;
    }

    // ------------------------------------------------------
    // 1. EnqueueRequest
    // ------------------------------------------------------
    public override Task<EnqueueResponseDto> EnqueueRequest(
        EnqueueRequestDto request,
        ServerCallContext context)
    {
        var requestId = Random.Shared.Next(1000, 9999);

        RequestStatuses[requestId] = "IN_QUEUE";

        _logger.LogInformation("New request {Id} enqueued", requestId);

        return Task.FromResult(new EnqueueResponseDto
        {
            RequestId = requestId,
            Status = "IN_QUEUE"
        });
    }

    // ------------------------------------------------------
    // 2. StreamData
    // ------------------------------------------------------
    public override async Task StreamData(
        StreamRequestDto request,
        IServerStreamWriter<StreamResponseDto> responseStream,
        ServerCallContext context)
    {
        var requestId = request.RequestId;

        if (!RequestStatuses.ContainsKey(requestId))
        {
            await responseStream.WriteAsync(new StreamResponseDto
            {
                RequestId = requestId,
                Status = StreamingStatus.Failed,
                ErrorMessage = "Request not found"
            });
            return;
        }

        _logger.LogInformation("Starting stream for request {Id}", requestId);

        const int totalCount = 25;
        const int pageSize = 5;
        int page = 1;

        try
        {
            for (int i = 0; i < totalCount; i += pageSize)
            {
                if (context.CancellationToken.IsCancellationRequested)
                {
                    _logger.LogWarning("Stream cancelled for request {Id}", requestId);
                    return;
                }

                var items = Enumerable.Range(i + 1, pageSize)
                    .Select(x => new DataItem
                    {
                        Id = x,
                        Value = $"Value {x}",
                        Description = $"Description for item {x}"
                    })
                    .ToList();

                var response = new StreamResponseDto
                {
                    RequestId = requestId,
                    TotalCount = totalCount,
                    Page = page,
                    Count = items.Count,
                    Status = StreamingStatus.InProgress
                };

                response.Data.AddRange(items);

                await responseStream.WriteAsync(response);

                await Task.Delay(300); // имитация задержки

                page++;
            }

            // send final
            await responseStream.WriteAsync(new StreamResponseDto
            {
                RequestId = requestId,
                TotalCount = totalCount,
                Page = page - 1,
                Count = 0,
                Status = StreamingStatus.Completed
            });

            _logger.LogInformation("Request {Id} completed", requestId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while streaming {Id}", requestId);

            await responseStream.WriteAsync(new StreamResponseDto
            {
                RequestId = requestId,
                Status = StreamingStatus.Failed,
                ErrorMessage = ex.Message
            });
        }
    }
}