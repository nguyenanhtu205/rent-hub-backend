namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractsForManager;

public record CustomerInfo(string Name, string Phone);

public record GetContractForManagerQuery : IRequest<List<ManagerContractVm>>;

public class GetContractForManagerQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetContractForManagerQuery, List<ManagerContractVm>>
{
    public async Task<List<ManagerContractVm>> Handle(GetContractForManagerQuery request,
        CancellationToken cancellationToken)
    {
        List<ConsignmentContract> contracts = await context.ConsignmentContracts
            .AsNoTracking()
            .Where(c => c.Status == ConsignmentContractStatus.PendingManagerApproval)
            .Include(c => c.Property)
            .ToListAsync(cancellationToken);

        if (contracts.Count == 0)
        {
            return [];
        }

        List<int> propertyIds = contracts.Select(c => c.PropertyId).ToList();

        Dictionary<int, CustomerInfo> customerInfo = await context.Properties
            .AsNoTracking()
            .Where(p => propertyIds.Contains(p.Id))
            .Select(p => new { p.Id, p.Customer!.Name, p.Customer.Phone })
            .ToDictionaryAsync(p => p.Id, p => new CustomerInfo(p.Name, p.Phone), cancellationToken);

        return contracts.Select(c => new ManagerContractVm
        {
            Clauses = DefaultContractTerms.Deserialize(c.Terms),
            CustomerName = customerInfo[c.PropertyId].Name,
            CustomerPhone = customerInfo[c.PropertyId].Phone,
            ContractId = c.Id,
            Type = c.Property!.Type,
            Address = c.Property.Address
        }).ToList();
    }
}
