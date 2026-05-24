namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForFinanceStaff;

public class UpdateContractForFinanceStaffCommandValidator : AbstractValidator<UpdateContractForFinanceStaffCommand>
{
    public UpdateContractForFinanceStaffCommandValidator()
    {
        RuleFor(x => x.ContractId)
            .GreaterThan(0).WithMessage("ContractId must be greater than 0");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Type must be a valid FinancialTransactionType");

        RuleFor(x => x.Method)
            .IsInEnum().WithMessage("Method must be a valid FinancialTransactionMethod");
    }
}
