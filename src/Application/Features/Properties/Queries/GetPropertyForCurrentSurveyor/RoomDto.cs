namespace Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;

public class RoomDto
{
    public required string Name { get; init; }

    public double Area { get; init; }

    public double Price { get; init; }

    public required RoomStatus Status { get; init; }

    public required string Notes { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Room, RoomDto>();
        }
    }
}
