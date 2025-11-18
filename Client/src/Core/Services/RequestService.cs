using Core.Clients.Abstractions;

using Database;

using Domain.Entities;
using Domain.Enums;

namespace Core.Services;

public class RequestService(AppDbContext context, IRequestQueue queue, IDataStreamingClient<ResponseItem> client) : IRequestService
{
    public async Task<int> CreateRequest(RequestDto dto)
    {
        var externalId = await client.GetRequestId(CancellationToken.None);

        var request = new RequestEntity
        {
            Status = RequestStatus.New,
            ExternalId = externalId,
        };

        context.Requests.Add(request);
        await context.SaveChangesAsync();

        await queue.EnqueueAsync(new QueueDto(request.Id, request.ExternalId));

        return request.Id;
    }
}