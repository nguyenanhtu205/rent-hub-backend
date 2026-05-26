namespace Application.Features.RentalTransactions.Queries.GetRentalTransactionForManager;

public class ManagerRentalTransactionVm
{
    public int Id { get; init; }

    public double Price { get; init; }

    public DateTimeOffset ClosedTime { get; init; }

    public required string LessorName { get; init; }

    public required string LessorPhone { get; init; }

    public required string BrokerName { get; init; }

    public double CommissionAmount { get; init; }
}
