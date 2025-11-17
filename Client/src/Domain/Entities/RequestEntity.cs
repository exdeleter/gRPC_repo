using Domain.Enums;

namespace Domain.Entities;

public class RequestEntity
{
    public int Id { get; set; }
    public RequestStatus Status { get; set; }
    public double Progress { get; set; }
    public string? ErrorMessage { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public ICollection<RequestResult> Results { get; set; } = new List<RequestResult>();
}