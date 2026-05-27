namespace Application.Features.Properties.Queries.GetFeaturedProperties;

public class FeaturedPropertyVm
{
    public required FeaturedPropertyDto PropertyDto { get; init; }

    public required string Url { get; init; }
}
