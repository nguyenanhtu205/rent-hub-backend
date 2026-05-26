namespace Application.Features.RentalTransactions.Queries.GetRentalTransactionForFinance;

public record GetRentalTransactionForFinanceQuery : IRequest<List<FinanceRentalTransactionVm>>;

public class GetRentalTransactionForFinanceQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetRentalTransactionForFinanceQuery, List<FinanceRentalTransactionVm>>
{
    public async Task<List<FinanceRentalTransactionVm>> Handle(GetRentalTransactionForFinanceQuery request,
        CancellationToken cancellationToken)
    {
        List<RentalTransaction> rentalTransaction = await context.RentalTransactions
            .AsNoTracking()
            .Where(rt => rt.Status == RentalTransactionStatus.PendingFinance)
            .OrderBy(rt => rt.ClosedTime)
            .ToListAsync(cancellationToken);

        if (rentalTransaction.Count == 0)
        {
            return [];
        }

        List<int> propertyIds = rentalTransaction.Select(rt => rt.PropertyId).ToList();

        Dictionary<int, Property> properties = await context.Properties
            .AsNoTracking()
            .Where(p => propertyIds.Contains(p.Id))
            .Include(p => p.Customer)
            .Include(p => p.ConsignmentContract)
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        return rentalTransaction.Select(rt => new FinanceRentalTransactionVm
        {
            Id = rt.Id,
            Price = rt.Price,
            ClosedTime = rt.ClosedTime,
            LessorName = properties[rt.PropertyId].Customer!.Name,
            LessorPhone = properties[rt.PropertyId].Customer!.Phone,
            CommissionAmount = rt.CommissionAmount,
            RemainingDeposit = properties[rt.PropertyId].ConsignmentContract!.RemainingDeposit
        }).ToList();
    }
}
