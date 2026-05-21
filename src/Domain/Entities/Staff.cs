namespace Domain.Entities;

public class Staff : BaseEntity
{
    public required string Name { get; set; }
    
    public required string Phone { get; set; }
    
    public required StaffRole Role { get; set; }
    
    public int ActiveWorkCount { get; set; }
    
    public int AccountId { get; set; }
    
    public Account? Account { get; set; }
    
    public ICollection<Property> Properties { get; private set; } = new List<Property>();
    
    public ICollection<FinancialTransaction> FinancialTransactions { get; private set; } = new List<FinancialTransaction>();
    
    public ICollection<RentalTransaction> RentalTransactions { get; private set; } = new List<RentalTransaction>();
    
    public ICollection<WorkHistory> WorkHistories { get; private set; } = new List<WorkHistory>();
    
    public ICollection<StaffWorkingArea> WorkingAreas { get; private set; } = new List<StaffWorkingArea>();
}
