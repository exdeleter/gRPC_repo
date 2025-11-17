using Database;
using Domain.Entities;
using Domain.Enums;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Core;

public class RequestProcessor : IRequestProcessor
{
    private readonly AppDbContext _db;
    private readonly IDataStreamingClient<ResponseItem> _streaming;
    // private readonly ILogger<RequestProcessor> _logger;

    public RequestProcessor(
        AppDbContext db,
        IDataStreamingClient<ResponseItem> streaming
        // ILogger<RequestProcessor> logger
        )
    {
        _db = db;
        _streaming = streaming;
        // _logger = logger;
    }

    public async Task ProcessAsync(int requestId, CancellationToken token)
    {
        var request = await _db.Requests.FindAsync([requestId], token);

        if (request is null)
        {
            // _logger.LogError("Request {Id} not found", requestId);
            return;
        }

        request.Status = RequestStatus.Processing;
        request.Progress = 0;
        await _db.SaveChangesAsync(token);

        try
        {
            int total = 0;
            int received = 0;

            await foreach (var page in _streaming.StreamAsync(requestId, token))
            {
                token.ThrowIfCancellationRequested();

                if (total == 0)
                    total = page.TotalCount;

                received += page.Count;

                // Save data
                foreach (var item in page.Data)
                {
                    _db.RequestResults.Add(new RequestResult
                    {
                        RequestId = requestId,
                        Key = item.Key,
                        Value = item.Value
                    });
                }

                // update progress
                if (total > 0)
                    request.Progress = Math.Min(100.0, (double)received / total * 100);

                await _db.SaveChangesAsync(token);
            }

            // Finalize
            request.Progress = 100;
            request.Status = RequestStatus.Success;
            await _db.SaveChangesAsync(token);
        }
        catch (Exception ex)
        {
            // _logger.LogError(ex, "Error processing request {Id}", requestId);

            request.Status = RequestStatus.Error;
            request.ErrorMessage = ex.Message;

            // remove inconsistent data
            await _db.Database.ExecuteSqlRawAsync(
                "DELETE FROM request_results WHERE request_id = {0}",
                requestId);

            await _db.SaveChangesAsync(token);
        }
    }
}