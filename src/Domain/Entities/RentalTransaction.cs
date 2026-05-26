namespace Domain.Entities;

public class RentalTransaction : BaseEntity
{
    public double Price { get; set; }
    
    public double CommissionRate { get; set; }
    
    public DateTimeOffset ClosedTime { get; set; }
    
    public required RentalTransactionStatus Status { get; set; }
    
    public int StaffId { get; set; }
    
    public int CustomerId { get; set; }
    
    public int PropertyId { get; set; }
    
    public Staff? Staff { get; set; }
    
    public Customer? Customer { get; set; }
    
    public ICollection<RoomRentalTransaction> RoomRentalTransactions { get; private set; } = new List<RoomRentalTransaction>();
}
