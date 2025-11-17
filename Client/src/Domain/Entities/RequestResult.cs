namespace Domain.Entities;

public class RequestResult
{
    public int Id { get; set; }

    public int RequestId { get; set; }
    public Request Request { get; set; } = null!;

    public string Key { get; set; } = null!;
    public string Value { get; set; } = null!;
}