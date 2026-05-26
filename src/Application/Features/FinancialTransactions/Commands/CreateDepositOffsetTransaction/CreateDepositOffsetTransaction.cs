namespace Application.Features.FinancialTransactions.Commands.CreateDepositOffsetTransaction;

public record CreateDepositOffsetTransactionCommand(double Amount, int RentalTransactionId) : IRequest;

public class CreateDepositOffsetTransactionCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<CreateDepositOffsetTransactionCommand>
{
    public async Task Handle(CreateDepositOffsetTransactionCommand request, CancellationToken cancellationToken)
    {
        RentalTransaction? rentalTransaction = await context.RentalTransactions
            .Where(rt => rt.Id == request.RentalTransactionId)
            .FirstOrDefaultAsync(cancellationToken);

        if (rentalTransaction == null)
        {
            throw new NotFoundException("Rental transaction not found.");
        }

        Property? property = await context.Properties
            .Where(p => p.Id == rentalTransaction.PropertyId)
            .Include(p => p.ConsignmentContract)
            .FirstOrDefaultAsync(cancellationToken);

        if (property == null)
        {
            throw new NotFoundException("Property not found");
        }

        await context.ConsignmentContracts
            .Where(c => c.Id == property.ConsignmentContract!.Id)
            .ExecuteUpdateAsync(c => c.SetProperty(cc
                => cc.RemainingDeposit, cc => cc.RemainingDeposit - request.Amount), cancellationToken);

        await context.RentalTransactions
            .Where(rt => rt.Id == request.RentalTransactionId)
            .ExecuteUpdateAsync(r => r.SetProperty(rt
                => rt.CommissionAmount, rt => rt.CommissionAmount - request.Amount), cancellationToken);

        context.FinancialTransactions.Add(new FinancialTransaction
        {
            Type = FinancialTransactionType.DepositOffset,
            Amount = request.Amount,
            Method = FinancialTransactionMethod.BankTransfer,
            Date = DateTimeOffset.UtcNow,
            RefType = RefType.ConsignmentContract,
            RefId = property.ConsignmentContract!.Id,
            StaffId = int.Parse(currentUser.Id!)
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}
