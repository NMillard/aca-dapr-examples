namespace Articles.RestApi.Domain;

public record UserInfo
{
    public required Guid UserId { get; init; }
    public required string Username { get; init; }
    public DateTimeOffset Created { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? LastUpdated { get; set; }
}