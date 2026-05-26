namespace Application.Features.RentalTransactions.Commands.CreateRentalTransaction;

public class CreateRentalTransactionCommandValidator : AbstractValidator<CreateRentalTransactionCommand>
{
    public CreateRentalTransactionCommandValidator()
    {
        RuleFor(x => x.WorkHistoryId)
            .GreaterThan(0).WithMessage("Work history ID must be greater than 0.");

        RuleFor(x => x.RoomIds)
            .NotEmpty().WithMessage("At least one room ID must be provided.")
            .Must(roomIds => roomIds.All(id => id > 0)).WithMessage("All room IDs must be greater than 0.");

        RuleFor(x => x.PropertyId)
            .GreaterThan(0).WithMessage("Property ID must be greater than 0.");
    }
}
