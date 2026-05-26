namespace Application.Features.RentalTransactions.Commands.UpdateRentalTransactionForFinance;

public record UpdateRentalTransactionForFinanceCommand(
    double Amount,
    int RentalTransactionId,
    FinancialTransactionMethod Method) : IRequest;

public class UpdateRentalTransactionForFinanceCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateRentalTransactionForFinanceCommand>
{
    public async Task Handle(UpdateRentalTransactionForFinanceCommand request, CancellationToken cancellationToken)
    {
        RentalTransaction? rentalTransaction =
            await context.RentalTransactions.FindAsync([request.RentalTransactionId], cancellationToken);

        if (rentalTransaction == null)
        {
            throw new NotFoundException("Rental transaction not found.");
        }

        rentalTransaction.Status = RentalTransactionStatus.PendingFinalApproval;

        if (request.Amount > 0)
        {
            context.FinancialTransactions.Add(new FinancialTransaction
            {
                Type = FinancialTransactionType.CommissionReceived,
                Amount = request.Amount,
                Method = request.Method,
                Date = DateTimeOffset.UtcNow,
                RefType = RefType.RentalTransaction,
                RefId = request.RentalTransactionId,
                StaffId = int.Parse(currentUser.Id!)
            });
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
