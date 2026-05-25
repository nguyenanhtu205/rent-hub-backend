namespace Application.Features.WorkHistories.Commands.UpdateViewingSchedule;

public class UpdateViewingScheduleCommandValidator : AbstractValidator<UpdateViewingScheduleCommand>
{
    public UpdateViewingScheduleCommandValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0");

        RuleFor(x => x.Time)
            .GreaterThan(DateTimeOffset.UtcNow).WithMessage("Time must be in the future");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status must be a valid enum value");
    }
}
