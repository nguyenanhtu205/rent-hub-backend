namespace Application.Features.FinancialTransactions.Queries.GetFinancialTransaction;

public record GetFinancialTransactionsQuery : IRequest<List<FinancialTransactionDto>>;

public class GetFinancialTransactionsQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUser currentUser)
    : IRequestHandler<GetFinancialTransactionsQuery, List<FinancialTransactionDto>>
{
    public async Task<List<FinancialTransactionDto>> Handle(GetFinancialTransactionsQuery request,
        CancellationToken cancellationToken)
    {
        string role = currentUser.Role!;

        if (role != nameof(StaffRole.FinanceStaff) && role != nameof(StaffRole.Manager))
        {
            throw new ForbiddenAccessException();
        }

        {
            return await context.FinancialTransactions
                .ProjectTo<FinancialTransactionDto>(mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
