namespace Application.Features.WorkHistories.Commands.CreateSurveySchedule;

public class CreateSurveyScheduleCommandValidator : AbstractValidator<CreateSurveyScheduleCommand>
{
    public CreateSurveyScheduleCommandValidator()
    {
        RuleFor(x => x.Time)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Time must be in the future");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0)
            .WithMessage("PropertyId must be greater than 0");
    }
}
