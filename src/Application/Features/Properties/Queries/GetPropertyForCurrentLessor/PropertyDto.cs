namespace Application.Features.Properties.Queries.GetPropertyForCurrentLessor;

public class PropertyDto
{
    public int Id { get; init; }

    public required PropertyType Type { get; init; }

    public required string Address { get; init; }
    
    public required PropertyStatus Status { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Property, PropertyDto>();
        }
    }
}
