namespace Domain.Entities;

public class Room : BaseEntity
{
    public required string Name { get; set; }
    
    public double Area { get; set; }
    
    public double Price { get; set; }
    
    public required RoomStatus Status { get; set; }
    
    public required string Notes { get; set; }
    
    public int PropertyId { get; set; }
    
    public Property? Property { get; set; }
    
    public ICollection<RoomRentalTransaction> RoomRentalTransactions { get; private set; } = new List<RoomRentalTransaction>();
}
