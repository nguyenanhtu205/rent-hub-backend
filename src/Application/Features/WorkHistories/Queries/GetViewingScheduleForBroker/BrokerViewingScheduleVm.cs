namespace Application.Features.WorkHistories.Queries.GetViewingScheduleForBroker;

public class BrokerViewingScheduleVm
{
    public int Id { get; init; }

    public required DateTimeOffset Time { get; init; }

    public required WorkHistoryStatus Status { get; init; }

    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public required PropertyType PropertyType { get; init; }

    public required District District { get; init; }

    public required string Address { get; init; }

    public required string IsConfirm { get; init; }
}
