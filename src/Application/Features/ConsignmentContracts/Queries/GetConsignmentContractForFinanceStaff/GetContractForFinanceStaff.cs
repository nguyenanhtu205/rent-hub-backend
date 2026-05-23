namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForFinanceStaff;

public record CustomerInfo(string Name, string Phone);

public record GetContractForFinanceStaffQuery : IRequest<List<FinanceContractVm>>;

public class GetContractForLegalStaffQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetContractForFinanceStaffQuery, List<FinanceContractVm>>
{
    public async Task<List<FinanceContractVm>> Handle(GetContractForFinanceStaffQuery request,
        CancellationToken cancellationToken)
    {
        List<ConsignmentContract> contracts = await context.ConsignmentContracts
            .Where(c => c.Status == ConsignmentContractStatus.PendingFinanceReview ||
                        c.Status == ConsignmentContractStatus.Cancelled)
            .Include(c => c.Property)
            .ToListAsync(cancellationToken);

        if (contracts.Count == 0)
        {
            return [];
        }

        List<int> propertyIds = contracts.Select(c => c.PropertyId).ToList();

        Dictionary<int, CustomerInfo> customerInfo = await context.Properties
            .Where(p => propertyIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Customer!.Name, p.Customer.Phone })
            .ToDictionaryAsync(p => p.Id, p => new CustomerInfo(p.Name, p.Phone), cancellationToken);

        return contracts.Select(c => new FinanceContractVm
        {
            ContractId = c.Id,
            CustomerName = customerInfo[c.PropertyId].Name,
            CustomerPhone = customerInfo[c.PropertyId].Phone,
            Status = c.Status,
            RemainingDeposit = c.RemainingDeposit,
            Type = c.Property!.Type,
            Address = c.Property.Address,
            Area = c.Property.Area,
            Direction = c.Property.Direction,
            NumOfRoom = c.Property.NumOfRoom
        }).ToList();
    }
}
