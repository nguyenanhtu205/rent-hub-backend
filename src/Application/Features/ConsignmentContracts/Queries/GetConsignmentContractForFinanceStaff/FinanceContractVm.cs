namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForFinanceStaff;

public class FinanceContractVm
{
    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public int ContractId { get; init; }
}
