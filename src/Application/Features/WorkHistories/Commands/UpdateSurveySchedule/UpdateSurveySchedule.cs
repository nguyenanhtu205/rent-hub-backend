namespace Application.Features.WorkHistories.Commands.UpdateSurveySchedule;

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

        if (workHistory.Time > DateTimeOffset.UtcNow)
        {
            throw new ConflictException("Survey schedule is in the future");
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

        await context.Staffs.Where(s => s.Id == int.Parse(currentUser.Id!))
            .ExecuteUpdateAsync(s => s.SetProperty(staff => staff.ActiveWorkCount, staff
                => staff.ActiveWorkCount - 1), cancellationToken);

        if (request.IsPassed)
        {
            await context.ConsignmentContracts.AddAsync(
                new ConsignmentContract
                {
                    DurationInMonths = DefaultContractTerms.DurationInMonths,
                    RemainingDeposit = 0,
                    CommissionRate = DefaultContractTerms.CommissionRate,
                    Terms = DefaultContractTerms.Serialize(),
                    Status = ConsignmentContractStatus.PendingLessorApproval,
                    PropertyId = int.Parse(workHistory.Note)
                }, cancellationToken);
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
