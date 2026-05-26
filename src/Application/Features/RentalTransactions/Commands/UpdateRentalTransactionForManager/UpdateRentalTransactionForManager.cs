namespace Application.Features.RentalTransactions.Commands.UpdateRentalTransactionForManager;

public record UpdateRentalTransactionForManagerCommand(int RentalTransactionId) : IRequest;

public class UpdateRentalTransactionForManagerCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateRentalTransactionForManagerCommand>
{
    public async Task Handle(UpdateRentalTransactionForManagerCommand request, CancellationToken cancellationToken)
    {
        RentalTransaction? rentalTransaction =
            await context.RentalTransactions.FindAsync([request.RentalTransactionId], cancellationToken);

        if (rentalTransaction == null)
        {
            throw new NotFoundException("Rental transaction not found.");
        }

        rentalTransaction.Status = RentalTransactionStatus.Completed;

        await context.SaveChangesAsync(cancellationToken);
    }
}
