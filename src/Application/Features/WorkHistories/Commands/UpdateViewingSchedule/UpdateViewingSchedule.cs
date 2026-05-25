namespace Application.Features.WorkHistories.Commands.UpdateViewingSchedule;

public record UpdateViewingScheduleCommand(int Id, DateTimeOffset Time, WorkHistoryStatus Status) : IRequest;

public class UpdateViewingScheduleCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateViewingScheduleCommand>
{
    public async Task Handle(UpdateViewingScheduleCommand request, CancellationToken cancellationToken)
    {
        WorkHistory? workHistory = await context.WorkHistories.FindAsync([request.Id], cancellationToken);

        if (workHistory == null)
        {
            throw new NotFoundException("Work history not found");
        }

        if (workHistory.StaffId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        if (request.Status == WorkHistoryStatus.Completed)
        {
            workHistory.Status = request.Status;

            await context.Staffs
                .Where(s => s.Id == int.Parse(currentUser.Id!))
                .ExecuteUpdateAsync(x => x.SetProperty(s
                    => s.ActiveWorkCount, s => s.ActiveWorkCount - 1), cancellationToken);
        }
        else
        {
            string[] parts = workHistory.Note.Split(':');
            int propertyId = int.Parse(parts[0]);

            workHistory.Time = request.Time;
            workHistory.Note = $"{propertyId}:1";
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
