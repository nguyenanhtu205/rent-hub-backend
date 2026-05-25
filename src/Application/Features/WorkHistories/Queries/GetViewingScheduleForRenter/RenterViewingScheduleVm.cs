namespace Application.Features.WorkHistories.Queries.GetViewingScheduleForRenter;

public class RenterViewingScheduleVm
{
    public required DateTimeOffset Time { get; init; }

    public required WorkHistoryStatus Status { get; init; }

    public required string StaffName { get; init; }

    public required string StaffPhone { get; init; }

    public required PropertyType PropertyType { get; init; }

    public required District District { get; init; }

    public required string Address { get; init; }

    public required string IsConfirm { get; init; }
}
