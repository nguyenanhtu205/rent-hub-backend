namespace Application.Features.WorkHistories.Queries.GetSurveyScheduleForLessor;

public class LessorSurveyScheduleVm
{
    public required PropertyType Type { get; init; }

    public required string PropertyAddress { get; init; }

    public DateTimeOffset Time { get; init; }

    public required WorkHistoryStatus Status { get; init; }

    public required string SurveyorName { get; init; }

    public required string Phone { get; init; }
}
