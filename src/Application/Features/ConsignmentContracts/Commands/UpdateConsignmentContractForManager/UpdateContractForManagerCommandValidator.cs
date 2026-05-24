namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForManager;

public class UpdateContractForManagerCommandValidator : AbstractValidator<UpdateContractForManagerCommand>
{
    public UpdateContractForManagerCommandValidator()
    {
        RuleFor(x => x.ContractId)
            .GreaterThan(0).WithMessage("ContractId must be greater than 0");
    }
}
