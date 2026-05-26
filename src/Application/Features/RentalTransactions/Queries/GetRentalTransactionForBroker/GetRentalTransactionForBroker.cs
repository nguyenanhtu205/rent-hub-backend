namespace Application.Features.RentalTransactions.Queries.GetRentalTransactionForBroker;

public record GetRentalTransactionForBrokerQuery : IRequest<List<BrokerRentalTransactionVm>>;

public class GetRentalTransactionForBrokerQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetRentalTransactionForBrokerQuery, List<BrokerRentalTransactionVm>>
{
    public async Task<List<BrokerRentalTransactionVm>> Handle(GetRentalTransactionForBrokerQuery request,
        CancellationToken cancellationToken)
    {
        List<RentalTransaction> transactions = await context.RentalTransactions
            .Where(rt => rt.StaffId == int.Parse(currentUser.Id!))
            .Include(rt => rt.Customer)
            .ToListAsync(cancellationToken);

        if (transactions.Count == 0)
        {
            return [];
        }

        return transactions.Select(rt => new BrokerRentalTransactionVm
        {
            Price = rt.Price,
            ClosedTime = rt.ClosedTime,
            CustomerName = rt.Customer!.Name,
            Status = rt.Status,
            CommissionAmount = rt.Price * rt.CommissionRate * rt.CommissionRate
        }).ToList();
    }
}
