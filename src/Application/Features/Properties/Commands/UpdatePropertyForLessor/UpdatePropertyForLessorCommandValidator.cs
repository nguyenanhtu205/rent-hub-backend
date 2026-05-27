namespace Application.Features.Properties.Commands.UpdatePropertyForLessor;

public class UpdatePropertyForLessorCommandValidator : AbstractValidator<UpdatePropertyForLessorCommand>
{
    public UpdatePropertyForLessorCommandValidator()
    {
        RuleFor(x => x.RoomIds)
            .NotNull().WithMessage("RoomIds cannot be null.")
            .NotEmpty().WithMessage("RoomIds cannot be empty.")
            .Must(roomIds => roomIds.All(id => id > 0)).WithMessage("All RoomIds must be greater than zero.");
    }
}
