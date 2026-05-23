namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLessor;

public class UpdateContractForLessorCommandValidator : AbstractValidator<UpdateContractForLessorCommand>
{
    public UpdateContractForLessorCommandValidator()
    {
        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("PropertyId must be greater than 0");

        RuleFor(x => x.State)
            .IsInEnum().WithMessage("Invalid state value");
    }
}
