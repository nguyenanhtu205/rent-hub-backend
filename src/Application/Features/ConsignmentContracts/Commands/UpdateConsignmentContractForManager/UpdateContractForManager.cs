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

        Property? property = await context.Properties
            .FirstOrDefaultAsync(x => x.Id == contract.PropertyId, cancellationToken);

        if (property == null)
        {
            throw new NotFoundException("Property not found");
        }

        property.Status = PropertyStatus.Active;

        int staffId = await context.StaffWorkingAreas
            .Where(x =>
                x.District == property.District &&
                x.Role == StaffRole.Broker)
            .Select(x => x.StaffId)
            .Distinct()
            .Join(
                context.Staffs,
                id => id,
                staff => staff.Id,
                (id, staff) => staff)
            .OrderBy(s => s.ActiveWorkCount)
            .Select(s => s.Id)
            .FirstAsync(cancellationToken);

        property.StaffId = staffId;

        contract.Status = ConsignmentContractStatus.Completed;
        contract.SigningDate = DateTimeOffset.UtcNow;

        await context.SaveChangesAsync(cancellationToken);
    }
}
