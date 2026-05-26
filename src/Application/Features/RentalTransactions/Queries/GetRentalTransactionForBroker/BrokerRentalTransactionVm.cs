namespace Application.Features.RentalTransactions.Queries.GetRentalTransactionForBroker;

public class BrokerRentalTransactionVm
{
    public double Price { get; init; }

    public DateTimeOffset ClosedTime { get; init; }

    public required string CustomerName { get; init; }

    public required RentalTransactionStatus Status { get; init; }

    public double CommissionAmount { get; init; }
}
