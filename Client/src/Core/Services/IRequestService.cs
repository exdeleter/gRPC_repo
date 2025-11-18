namespace Core.Services;

public interface IRequestService
{
    Task<int> CreateRequest(RequestDto dto);
}