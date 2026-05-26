namespace Application.Features.FinancialTransactions.Commands.CreateDepositOffsetTransaction;

public class CreateDepositOffsetTransactionCommandValidator : AbstractValidator<CreateDepositOffsetTransactionCommand>
{
    public CreateDepositOffsetTransactionCommandValidator()
    {
        RuleFor(c => c.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0");

        RuleFor(c => c.RentalTransactionId)
            .GreaterThan(0).WithMessage("Rental transaction ID must be greater than 0");
    }
}
