namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractsForManager;

public class ManagerContractVm
{
    public required IReadOnlyList<ContractClause> Clauses { get; init; }

    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public int ContractId { get; init; }

    public required PropertyType Type { get; init; }

    public required string Address { get; init; }
}
