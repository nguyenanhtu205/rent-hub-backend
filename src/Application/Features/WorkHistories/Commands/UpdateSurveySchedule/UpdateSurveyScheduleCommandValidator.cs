namespace Application.Features.WorkHistories.Commands.UpdateSurveySchedule;

public class UpdateSurveyScheduleCommandValidator : AbstractValidator<UpdateSurveyScheduleCommand>
{
    public UpdateSurveyScheduleCommandValidator()
    {
        RuleFor(x => x.WorkHistoryId)
            .GreaterThan(0).WithMessage("Work history ID must be greater than 0");

        RuleFor(x => x.IsPassed)
            .NotNull().WithMessage("IsPassed must not be null");
    }
}
