namespace Domain.Entities;

public class FinancialTransaction : BaseEntity
{
    public required FinancialTransactionType Type { get; set; }
    
    public double Amount { get; set; }
    
    public required FinancialTransactionMethod Method { get; set; }
    
    public DateTimeOffset Date { get; set; }
    
    public required RefType RefType { get; set; }
    
    public int RefId { get; set; }
    
    public int StaffId { get; set; }
    
    public Staff? Staff { get; set; }
}
