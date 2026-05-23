namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLegalStaff;

public class LegalContractVm
{
    public required IReadOnlyList<ContractClause> Clauses { get; init; }

    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public int ContractId { get; init; }
}
