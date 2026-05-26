namespace Application.Features.RentalTransactions.Queries.GetRentalTransactionForFinance;

public class FinanceRentalTransactionVm
{
    public int Id { get; init; }

    public double Price { get; init; }

    public DateTimeOffset ClosedTime { get; init; }

    public required string LessorName { get; init; }

    public required string LessorPhone { get; init; }

    public double CommissionAmount { get; init; }

    public double RemainingDeposit { get; init; }
}
