namespace Domain.Entities;

public class ConsignmentContractDocument : BaseEntity
{
    public required string Name { get; set; }
    
    public required string Url { get; set; }
    
    public int ConsignmentContractId { get; set; }
    
    public ConsignmentContract? ConsignmentContract { get; set; }
}
