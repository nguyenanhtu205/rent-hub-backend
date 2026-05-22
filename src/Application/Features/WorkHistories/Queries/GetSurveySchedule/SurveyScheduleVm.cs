namespace Application.Features.WorkHistories.Queries.GetSurveySchedule;

public class SurveyScheduleVm
{
    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public required string PropertyAddress { get; init; }

    public DateTimeOffset Time { get; set; }

    public required WorkHistoryStatus Status { get; set; }
}
