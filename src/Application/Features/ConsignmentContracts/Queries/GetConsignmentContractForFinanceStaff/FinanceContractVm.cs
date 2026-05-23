namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForFinanceStaff;

public class FinanceContractVm
{
    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public int ContractId { get; init; }

    public required ConsignmentContractStatus Status { get; init; }

    public double RemainingDeposit { get; init; }

    public required PropertyType Type { get; init; }

    public required string Address { get; init; }

    public double Area { get; init; }

    public required string Direction { get; init; }

    public int NumOfRoom { get; init; }
}
