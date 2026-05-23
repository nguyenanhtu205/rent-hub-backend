namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLessor;

public enum State
{
    Confirm,
    LegalCheck,
    CancelContract
}

public record UpdateContractForLessorCommand(int PropertyId, State State) : IRequest;

public class UpdateContractForLessorCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateContractForLessorCommand>
{
    public async Task Handle(UpdateContractForLessorCommand request, CancellationToken cancellationToken)
    {
        int lessorId = await context.Properties.Where(p => p.Id == request.PropertyId)
            .Select(p => p.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (lessorId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        ConsignmentContract? contract = await context.ConsignmentContracts
            .Where(c => c.PropertyId == request.PropertyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (contract == null)
        {
            throw new NotFoundException("Contract not found");
        }

        contract.Status = request.State switch
        {
            State.Confirm => ConsignmentContractStatus.PendingFinanceReview,
            State.LegalCheck => ConsignmentContractStatus.PendingLegalReview,
            State.CancelContract => ConsignmentContractStatus.Cancelled,
            _ => contract.Status
        };

        await context.SaveChangesAsync(cancellationToken);
    }
}
