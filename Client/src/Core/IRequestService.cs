namespace Core;

public interface IRequestService
{
    Task<int> CreateRequest(RequestDto dto);
}