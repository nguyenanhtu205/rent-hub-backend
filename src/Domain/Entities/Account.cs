namespace Domain.Entities;

public class Account : BaseEntity
{
    public required string Email { get; set; }
    
    public required string Password { get; set; }
    
    public required AccountStatus Status { get; set; }
    
    public required AccountRole Role { get; set; }
    
    public Staff? Staff { get; set; }
    
    public Customer? Customer { get; set; }
    
    public ICollection<RefreshToken> RefreshTokens { get; private set; } = new List<RefreshToken>();
}
