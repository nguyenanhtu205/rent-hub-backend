namespace Application.Features.Properties.Queries.GetPropertyForCurrentLessor;

public class PropertyVm
{
    public IReadOnlyCollection<PropertyDto> Properties { get; init; } = [];
}
