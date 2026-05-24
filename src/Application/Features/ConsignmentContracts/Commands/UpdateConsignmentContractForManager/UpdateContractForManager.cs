namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForManager;

public record UpdateContractForManagerCommand(int ContractId) : IRequest;

public class UpdateContractForManagerCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateContractForManagerCommand>
{
    public async Task Handle(UpdateContractForManagerCommand request, CancellationToken cancellationToken)
    {
        ConsignmentContract? contract = await context.ConsignmentContracts
            .FirstOrDefaultAsync(x => x.Id == request.ContractId, cancellationToken);

        if (contract == null)
        {
            throw new NotFoundException("Contract not found");
        }

        await context.Properties
            .Where(x => x.Id == contract.PropertyId)
            .ExecuteUpdateAsync(x => x.SetProperty(p
                => p.Status, PropertyStatus.Active), cancellationToken);

        contract.Status = ConsignmentContractStatus.Completed;
        contract.SigningDate = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
