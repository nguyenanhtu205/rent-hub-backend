namespace Application.Features.WorkHistories.Commands.CreateSurveySchedule;

public record CreateSurveyScheduleCommand(DateTimeOffset Time, int PropertyId) : IRequest;

public class CreateSurveyScheduleCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<CreateSurveyScheduleCommand>
{
    public async Task Handle(CreateSurveyScheduleCommand request, CancellationToken cancellationToken)
    {
        int surveyorId = int.Parse(currentUser.Id!);

        int updated = await context.Properties
            .Where(p => p.Id == request.PropertyId)
            .ExecuteUpdateAsync(x => x.SetProperty(
                    p => p.Status,
                    PropertyStatus.SurveyScheduled),
                cancellationToken);

        if (updated == 0)
        {
            throw new NotFoundException("Property not found");
        }

        await context.Staffs.Where(s => s.Id == surveyorId)
            .ExecuteUpdateAsync(x => x.SetProperty(s
                => s.ActiveWorkCount, s => s.ActiveWorkCount + 1), cancellationToken);

        int customerId = await context.Properties
            .Where(p => p.Id == request.PropertyId)
            .Select(p => p.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        WorkHistory workHistory = new()
        {
            Type = WorkHistoryType.SurveyorTask,
            Time = request.Time,
            Note = request.PropertyId.ToString(),
            Status = WorkHistoryStatus.Pending,
            StaffId = surveyorId,
            CustomerId = customerId
        };

        context.WorkHistories.Add(workHistory);

        await context.SaveChangesAsync(cancellationToken);
    }
}
