namespace Application.Features.WorkHistories.Queries.GetSurveySchedule;

public class SurveyScheduleVm
{
    public int Id { get; init; }
    
    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public required string PropertyAddress { get; init; }

    public DateTimeOffset Time { get; init; }

    public required WorkHistoryStatus Status { get; init; }
}
