namespace Domain.Entities;

public class AuditLog : BaseEntity
{
    public string RequestName { get; set; } = string.Empty;

    public string? UserId { get; set; }

    public string RequestPayload { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
