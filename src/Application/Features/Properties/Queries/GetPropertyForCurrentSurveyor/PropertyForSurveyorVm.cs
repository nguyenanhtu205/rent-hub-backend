namespace Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;

public class PropertyForSurveyorVm
{
    public required PropertyDto Property { get; init; }

    public IReadOnlyCollection<RoomDto> Rooms { get; init; } = [];

    public IReadOnlyCollection<DocumentDto> Documents { get; init; } = [];

    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }
}
