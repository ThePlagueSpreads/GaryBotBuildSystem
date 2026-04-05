namespace Core;

public class BuildRequest
{
    public Guid Id { get; init; } = Guid.NewGuid();
    
    public required string RequestedBy { get; init; }
    
    public DateTimeOffset RequestedAt { get; init; } = DateTimeOffset.UtcNow;

    public List<string> UploadLinkUrls { get; } = [];

    public string Status { get; set; } = "Requested";
}