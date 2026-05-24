namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForFinanceStaff;

public record UpdateContractForFinanceStaffCommand(
    int ContractId,
    FinancialTransactionType Type,
    double Amount,
    FinancialTransactionMethod Method) : IRequest;

public class UpdateContractForFinanceStaffCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateContractForFinanceStaffCommand>
{
    public async Task Handle(UpdateContractForFinanceStaffCommand request, CancellationToken cancellationToken)
    {
        if (request.Type == FinancialTransactionType.DepositReceived)
        {
            await context.ConsignmentContracts.Where(c => c.Id == request.ContractId)
                .ExecuteUpdateAsync(c => c.SetProperty(x
                    => x.Status, ConsignmentContractStatus.PendingManagerApproval), cancellationToken);

            context.FinancialTransactions.Add(new FinancialTransaction
            {
                Type = FinancialTransactionType.DepositReceived,
                Amount = request.Amount,
                Method = request.Method,
                Date = DateTimeOffset.UtcNow,
                RefType = RefType.ConsignmentContract,
                RefId = request.ContractId,
                StaffId = int.Parse(currentUser.Id!)
            });
        }
        else
        {
            context.FinancialTransactions.Add(new FinancialTransaction
            {
                Type = FinancialTransactionType.DepositRefunded,
                Amount = request.Amount,
                Method = request.Method,
                Date = DateTimeOffset.UtcNow,
                RefType = RefType.ConsignmentContract,
                RefId = request.ContractId,
                StaffId = int.Parse(currentUser.Id!)
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
