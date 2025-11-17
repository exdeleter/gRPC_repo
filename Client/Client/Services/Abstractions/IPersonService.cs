namespace Client.Services.Abstractions;

public interface IPersonService
{
    Task<string> Get();
}