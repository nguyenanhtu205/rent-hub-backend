namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLessor;

public class ConsignmentContractVm
{
    public DateTimeOffset? SigningDate { get; init; }

    public double RemainingDeposit { get; init; }

    public required ConsignmentContractStatus Status { get; init; }

    public required IReadOnlyList<ContractClause> Clauses { get; init; }
}
