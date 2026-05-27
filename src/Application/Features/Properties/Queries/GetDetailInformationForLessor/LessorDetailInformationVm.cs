namespace Application.Features.Properties.Queries.GetDetailInformationForLessor;

public record LessorRoomDto(int Id, string Name, double Area, double Price, RoomStatus Status, string Notes);

public class LessorDetailInformationVm
{
    public required PropertyType Type { get; init; }

    public double Area { get; init; }

    public required string Direction { get; init; }

    public int NumOfRoom { get; init; }

    public required string Address { get; init; }

    public required District District { get; init; }

    public required PropertyStatus Status { get; set; }

    public double Price { get; init; }

    public required string StaffName { get; init; }

    public required string StaffPhone { get; init; }

    public required ICollection<LessorRoomDto> Rooms { get; init; }

    public required ICollection<string> DocumentUrls { get; init; }
}
