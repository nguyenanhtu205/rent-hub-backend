namespace Domain.Entities;

public class RoomRentalTransaction : BaseEntity
{
    public int RoomId { get; set; }
    
    public int RentalTransactionId { get; set; }
    
    public Room? Room { get; set; }
    
    public RentalTransaction? RentalTransaction { get; set; }
}
