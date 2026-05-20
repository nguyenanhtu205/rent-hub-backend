namespace Domain.Entities;

public class Property : BaseEntity
{
    public required PropertyType Type { get; set; }
    
    public double Area { get; set; }
    
    public required string Direction { get; set; }
    
    public int NumOfRoom { get; set; }
    
    public required string Address { get; set; }
    
    public required District District { get; set; }
    
    public required PropertyStatus Status { get; set; }
    
    public int StaffId { get; set; }
    
    public int CustomerId { get; set; }
    
    public Staff? Staff { get; set; }
    
    public Customer? Customer { get; set; }
    
    public ConsignmentContract? ConsignmentContract { get; set; }
    
    public ICollection<Room> Rooms { get; private set; } = new List<Room>();
    
    public ICollection<PropertyDocument> PropertyDocuments { get; private set; } = new List<PropertyDocument>();
}
