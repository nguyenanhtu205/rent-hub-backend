namespace Application.Features.RentalTransactions.Commands.UpdateRentalTransactionForFinance;

public class UpdateRentalTransactionForFinanceCommandValidator
    : AbstractValidator<UpdateRentalTransactionForFinanceCommand>
{
    public UpdateRentalTransactionForFinanceCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0).WithMessage("Amount must be greater than or equal to 0.");

        RuleFor(x => x.RentalTransactionId)
            .GreaterThan(0).WithMessage("Rental transaction ID must be greater than 0.");

        RuleFor(x => x.Method)
            .IsInEnum().WithMessage("Invalid financial transaction method.");
    }
}
