namespace Application.Features.FinancialTransactions.Queries.GetFinancialTransaction;

public class FinancialTransactionDto
{
    public int Id { get; init; }

    public required FinancialTransactionType Type { get; init; }

    public double Amount { get; init; }

    public required FinancialTransactionMethod Method { get; init; }

    public DateTimeOffset Date { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<FinancialTransaction, FinancialTransactionDto>();
        }
    }
}
