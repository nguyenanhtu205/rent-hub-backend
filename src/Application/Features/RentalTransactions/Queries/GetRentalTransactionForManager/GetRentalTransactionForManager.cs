namespace Application.Features.RentalTransactions.Queries.GetRentalTransactionForManager;

public record GetRentalTransactionForManagerQuery : IRequest<List<ManagerRentalTransactionVm>>;

public class GetRentalTransactionForManagerQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetRentalTransactionForManagerQuery, List<ManagerRentalTransactionVm>>
{
    public async Task<List<ManagerRentalTransactionVm>> Handle(GetRentalTransactionForManagerQuery request,
        CancellationToken cancellationToken)
    {
        List<RentalTransaction> rentalTransaction = await context.RentalTransactions
            .AsNoTracking()
            .Where(rt => rt.Status == RentalTransactionStatus.PendingFinalApproval)
            .Include(rt => rt.Staff)
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
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        return rentalTransaction.Select(rt => new ManagerRentalTransactionVm
        {
            Id = rt.Id,
            Price = rt.Price,
            ClosedTime = rt.ClosedTime,
            LessorName = properties[rt.PropertyId].Customer!.Name,
            LessorPhone = properties[rt.PropertyId].Customer!.Phone,
            BrokerName = rt.Staff!.Name,
            CommissionAmount = rt.Price * rt.CommissionRate
        }).ToList();
    }
}
