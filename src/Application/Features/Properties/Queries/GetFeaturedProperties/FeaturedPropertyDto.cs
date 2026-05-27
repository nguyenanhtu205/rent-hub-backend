namespace Application.Features.Properties.Queries.GetFeaturedProperties;

public class FeaturedPropertyDto
{
    public int Id { get; init; }

    public required PropertyType Type { get; init; }

    public double Area { get; init; }

    public required string Direction { get; init; }

    public int NumOfRoom { get; init; }

    public required District District { get; init; }

    public double Price { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Property, FeaturedPropertyDto>();
        }
    }
}
