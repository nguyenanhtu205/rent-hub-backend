namespace Domain.Entities;

public class ConsignmentContract : BaseEntity
{
    public DateTimeOffset? SigningDate { get; set; }
    
    public int DurationInMonths { get; set; }
    
    public double RemainingDeposit { get; set; }
    
    public double CommissionRate { get; set; }
    
    public required string Terms { get; set; }
    
    public required ConsignmentContractStatus Status { get; set; }
    
    public int PropertyId { get; set; }
    
    public Property? Property { get; set; }
    
    public ICollection<ConsignmentContractDocument> Documents { get; private set; } = new List<ConsignmentContractDocument>();
}
