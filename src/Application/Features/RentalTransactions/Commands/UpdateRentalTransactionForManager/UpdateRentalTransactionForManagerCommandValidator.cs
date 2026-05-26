namespace Application.Features.RentalTransactions.Commands.UpdateRentalTransactionForManager;

public class UpdateRentalTransactionForManagerCommandValidator
    : AbstractValidator<UpdateRentalTransactionForManagerCommand>
{
    public UpdateRentalTransactionForManagerCommandValidator()
    {
        RuleFor(x => x.RentalTransactionId)
            .GreaterThan(0).WithMessage("Rental transaction ID must be greater than 0.");
    }
}
