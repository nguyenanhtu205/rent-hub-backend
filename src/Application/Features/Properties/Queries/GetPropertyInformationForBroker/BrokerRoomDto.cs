namespace Application.Features.Properties.Queries.GetPropertyInformationForBroker;

public class BrokerRoomDto
{
    public int Id { get; init; }

    public required string Name { get; init; }

    public double Price { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Room, BrokerRoomDto>();
        }
    }
}
