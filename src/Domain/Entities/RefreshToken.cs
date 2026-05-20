namespace Domain.Entities;

public class RefreshToken : BaseEntity
{
    public int AccountId { get; set; }

    public required string Token { get; set; }

    public DateTimeOffset? ExpiredAt { get; set; }

    public bool IsRevoked { get; set; }

    public int? ReplacedByTokenId { get; set; }

    public RefreshToken? ReplacedByToken { get; set; }

    public Account? Account { get; set; }
}
