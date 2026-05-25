namespace Application.Features.WorkHistories.Commands.CreateViewingSchedule;

public record CreateViewingScheduleCommand(int PropertyId, DateTimeOffset Time) : IRequest;

public class CreateViewingScheduleCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<CreateViewingScheduleCommand>
{
    public async Task Handle(CreateViewingScheduleCommand request, CancellationToken cancellationToken)
    {
        Property? property = await context.Properties.FindAsync([request.PropertyId], cancellationToken);

        if (property == null)
        {
            throw new NotFoundException("Property not found");
        }

        bool exists = await context.WorkHistories
            .AnyAsync(w => w.Type == WorkHistoryType.BrokerTask
                           && w.CustomerId == int.Parse(currentUser.Id!)
                           && w.Note == request.PropertyId.ToString()
                           && w.Status == WorkHistoryStatus.Pending, cancellationToken);
        if (exists)
        {
            throw new ConflictException("You have already scheduled a viewing for this property");
        }

        context.WorkHistories.Add(new WorkHistory
        {
            Type = WorkHistoryType.BrokerTask,
            Time = request.Time,
            Note = request.PropertyId.ToString(),
            Status = WorkHistoryStatus.Pending,
            StaffId = property.StaffId,
            CustomerId = int.Parse(currentUser.Id!)
        });

        await context.Staffs
            .Where(s => s.Id == property.StaffId)
            .ExecuteUpdateAsync(x => x.SetProperty(s
                => s.ActiveWorkCount, s => s.ActiveWorkCount + 1), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
