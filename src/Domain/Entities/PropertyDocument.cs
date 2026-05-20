namespace Domain.Entities;

public class PropertyDocument : BaseEntity
{
    public required string Name { get; set; }
    
    public required PropertyDocumentType Type { get; set; }
    
    public required string Url { get; set; }
    
    public int PropertyId { get; set; }
    
    public Property? Property { get; set; }
}
