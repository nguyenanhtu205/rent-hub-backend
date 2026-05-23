namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLegalStaff;

public class UpdateContractForLegalStaffCommandValidator : AbstractValidator<UpdateContractForLegalStaffCommand>
{
    public UpdateContractForLegalStaffCommandValidator()
    {
        RuleFor(c => c.ContractId)
            .GreaterThan(0).WithMessage("Contract ID must be greater than 0.");

        RuleForEach(c => c.AdditionalClauses)
            .NotNull().WithMessage("Additional clauses cannot be null")
            .NotEmpty().WithMessage("Additional clauses cannot be empty")
            .Must(clause => !string.IsNullOrWhiteSpace(clause.Title))
            .WithMessage("Clause title cannot be empty")
            .Must(clause => !string.IsNullOrWhiteSpace(clause.Content))
            .WithMessage("Clause content cannot be empty");
    }
}
