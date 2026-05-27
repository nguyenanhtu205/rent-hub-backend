namespace Application.Features.WorkHistories.Queries.GetAllWorkHistories;

public class AllWorkHistoryVm
{
    public required WorkHistoryType Type { get; init; }

    public DateTimeOffset Time { get; init; }

    public required WorkHistoryStatus Status { get; init; }

    public required string StaffName { get; init; }

    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public IReadOnlyList<ContractClause>? AdditionalClauses { get; init; }

    public string? Address { get; init; }

    public PropertyType? PropertyType { get; init; }

    public District? District { get; set; }

    public string? LessorName { get; init; }

    public string? LessorPhone { get; init; }
}
