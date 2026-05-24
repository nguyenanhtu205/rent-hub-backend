namespace Application.Features.Properties.Queries.GetDetailInformation;

public record RoomDto(string Name, double Area, double Price, RoomStatus Status, string Notes);

public class PropertyInformationVm
{
    public int Id { get; init; }
    
    public required PropertyType Type { get; init; }

    public double Area { get; init; }

    public required string Direction { get; init; }

    public int NumOfRoom { get; init; }

    public required string Address { get; init; }

    public required District District { get; init; }

    public double Price { get; init; }

    public required string StaffName { get; init; }

    public required string StaffPhone { get; init; }

    public required ICollection<RoomDto> Rooms { get; init; }

    public required ICollection<string> DocumentUrls { get; init; }
}
