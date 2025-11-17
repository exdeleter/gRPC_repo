using Database;

using Domain.Entities;
using Domain.Enums;

namespace Core;

public class RequestService(AppDbContext context, IRequestQueue queue) : IRequestService
{
    public async Task<int> CreateRequest(RequestDto dto)
    {
        var request = new RequestEntity()
        {
            Status = RequestStatus.New
        };

        context.Requests.Add(request);
        await context.SaveChangesAsync();

        await queue.EnqueueAsync(request.Id);

        return request.Id;
    }
}