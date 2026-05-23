namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForFinanceStaff;

public class FinanceContractVm
{
    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public int ContractId { get; init; }
    
    public required ConsignmentContractStatus Status { get; set; }
    
    public double RemainingDeposit { get; set; }
}
