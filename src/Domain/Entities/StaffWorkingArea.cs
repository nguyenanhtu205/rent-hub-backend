namespace Domain.Entities;

public class StaffWorkingArea : BaseEntity
{
    public int StaffId { get; set; }
    
    public required StaffRole Role { get; set; }
    
    public required District District { get; set; }
    
    public Staff? Staff { get; set; }
}
