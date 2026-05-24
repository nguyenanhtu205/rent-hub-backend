namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLessor;

public record GetConsignmentContractsForLessorQuery(int PropertyId) : IRequest<ConsignmentContractVm>;

public class GetConsignmentContractsForLessorQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUser currentUser)
    : IRequestHandler<GetConsignmentContractsForLessorQuery, ConsignmentContractVm>
{
    public async Task<ConsignmentContractVm> Handle(GetConsignmentContractsForLessorQuery request,
        CancellationToken cancellationToken)
    {
        int lessorId = await context.Properties
            .AsNoTracking()
            .Where(x => x.Id == request.PropertyId)
            .Select(x => x.CustomerId)
            .FirstOrDefaultAsync(cancellationToken);

        if (lessorId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        ConsignmentContractDto? contract = await context.ConsignmentContracts
            .AsNoTracking()
            .Where(x => x.PropertyId == request.PropertyId)
            .ProjectTo<ConsignmentContractDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (contract == null)
        {
            throw new NotFoundException("Consignment contract not found");
        }

        return new ConsignmentContractVm
        {
            SigningDate = contract.SigningDate,
            RemainingDeposit = contract.RemainingDeposit,
            Status = contract.Status,
            Clauses = DefaultContractTerms.Deserialize(contract.Terms)
        };
    }
}
