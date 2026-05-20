namespace Domain.Entities;

public class WorkHistory : BaseEntity
{
    public required WorkHistoryType Type { get; set; }
    
    public DateTimeOffset Time { get; set; }
    
    public required string Note { get; set; }
    
    public required WorkHistoryStatus Status { get; set; }
    
    public int StaffId { get; set; }
    
    public int CustomerId { get; set; }
    
    public Staff? Staff { get; set; }
    
    public Customer? Customer { get; set; }
}
