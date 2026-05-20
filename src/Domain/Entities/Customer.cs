namespace Domain.Entities;

public class Customer : BaseEntity
{
    public required string Name { get; set; }
    
    public required string Phone { get; set; }
    
    public required string CitizenId { get; set; }
    
    public required CustomerType Type { get; set; }
    
    public int AccountId { get; set; }
    
    public Account? Account { get; set; }
    
    public ICollection<Property> Properties { get; private set; } = new List<Property>();
    
    public ICollection<WorkHistory> WorkHistories { get; private set; } = new List<WorkHistory>();
    
    public ICollection<RentalTransaction> RentalTransactions { get; private set; } = new List<RentalTransaction>();
}
