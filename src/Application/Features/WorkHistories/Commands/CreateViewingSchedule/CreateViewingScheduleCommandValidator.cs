namespace Application.Features.WorkHistories.Commands.CreateViewingSchedule;

public class CreateViewingScheduleCommandValidator : AbstractValidator<CreateViewingScheduleCommand>
{
    public CreateViewingScheduleCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("PropertyId must be greater than 0");

        RuleFor(x => x.Time)
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Time must be in the future");
    }
}
