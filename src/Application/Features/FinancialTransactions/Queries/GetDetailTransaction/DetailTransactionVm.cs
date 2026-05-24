namespace Application.Features.FinancialTransactions.Queries.GetDetailTransaction;

public record PropertyDto
{
    public required PropertyType Type { get; init; }

    public required string Address { get; init; }
}

public record RentalTransactionDto
{
    public double Price { get; init; }

    public double CommissionRate { get; init; }

    public DateTimeOffset ClosedTime { get; init; }
}

public class DetailTransactionVm
{
    public required FinancialTransactionType Type { get; init; }

    public double Amount { get; init; }

    public required FinancialTransactionMethod Method { get; init; }

    public DateTimeOffset Date { get; init; }

    public required RefType RefType { get; init; }

    public required string CustomerName { get; init; }

    public required string CustomerPhone { get; init; }

    public required string StaffName { get; init; }

    public PropertyDto? Property { get; init; }

    public RentalTransactionDto? RentalTransaction { get; set; }
}
