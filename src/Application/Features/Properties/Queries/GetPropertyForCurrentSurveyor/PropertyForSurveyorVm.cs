namespace Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;

public class PropertyForSurveyorVm
{
    public required PropertyDto Property { get; init; }

    public IReadOnlyCollection<RoomDto> Rooms { get; init; } = [];

    public IReadOnlyCollection<DocumentDto> Documents { get; init; } = [];
}
