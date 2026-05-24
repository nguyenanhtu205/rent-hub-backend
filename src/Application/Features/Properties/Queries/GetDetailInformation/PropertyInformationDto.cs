namespace Application.Features.Properties.Queries.GetDetailInformation;

public class PropertyInformationDto
{
    public int Id { get; init; }
    
    public required PropertyType Type { get; init; }

    public double Area { get; init; }

    public required string Direction { get; init; }

    public int NumOfRoom { get; init; }

    public required string Address { get; init; }

    public required District District { get; init; }

    public double Price { get; init; }

    public Staff? Staff { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Property, PropertyInformationDto>();
        }
    }
}
