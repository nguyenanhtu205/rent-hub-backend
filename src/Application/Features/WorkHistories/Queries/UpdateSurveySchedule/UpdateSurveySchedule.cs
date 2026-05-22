namespace Application.Features.WorkHistories.Queries.UpdateSurveySchedule;

public record UpdateSurveyScheduleCommand(int WorkHistoryId, bool IsPassed) : IRequest;

public class UpdateSurveyScheduleCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdateSurveyScheduleCommand>
{
    public async Task Handle(UpdateSurveyScheduleCommand request, CancellationToken cancellationToken)
    {
        WorkHistory? workHistory = await context.WorkHistories
            .FirstOrDefaultAsync(x => x.Id == request.WorkHistoryId, cancellationToken);

        if (workHistory is null)
        {
            throw new NotFoundException("Work history not found");
        }

        if (workHistory.StaffId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        if (workHistory.Status == WorkHistoryStatus.Completed)
        {
            throw new ConflictException("Work history already completed");
        }

        workHistory.Status = WorkHistoryStatus.Completed;

        PropertyStatus newStatus = request.IsPassed ? PropertyStatus.Surveyed : PropertyStatus.NotSurveyed;
        await context.Properties
            .Where(x => x.Id == int.Parse(workHistory.Note))
            .ExecuteUpdateAsync(x => x.SetProperty(p
                => p.Status, newStatus), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
