namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLegalStaff;

public record CustomerInfo(string Name, string Phone);

public record GetContractForLegalStaffQuery : IRequest<List<LegalContractVm>>;

public class GetContractForLegalStaffQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetContractForLegalStaffQuery, List<LegalContractVm>>
{
    public async Task<List<LegalContractVm>> Handle(GetContractForLegalStaffQuery request,
        CancellationToken cancellationToken)
    {
        List<ConsignmentContract> contracts = await context.ConsignmentContracts
            .Where(c => c.Status == ConsignmentContractStatus.PendingLegalReview)
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

        return contracts.Select(c => new LegalContractVm
        {
            ContractId = c.Id,
            Clauses = DefaultContractTerms.Deserialize(c.Terms),
            CustomerName = customerInfo[c.PropertyId].Name,
            CustomerPhone = customerInfo[c.PropertyId].Phone
        }).ToList();
    }
}
