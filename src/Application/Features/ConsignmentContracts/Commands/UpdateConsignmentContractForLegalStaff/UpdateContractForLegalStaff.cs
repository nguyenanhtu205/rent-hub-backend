namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLegalStaff;

public record UpdateContractForLegalStaffCommand(int ContractId, List<ContractClause> AdditionalClauses) : IRequest;

public class UpdateContractForLegalStaffCommandHandler(IApplicationDbContext context)
    : IRequestHandler<UpdateContractForLegalStaffCommand>
{
    public async Task Handle(
        UpdateContractForLegalStaffCommand request,
        CancellationToken cancellationToken)
    {
        ConsignmentContract? contract = await context.ConsignmentContracts
            .FirstOrDefaultAsync(c => c.Id == request.ContractId, cancellationToken);

        if (contract is null)
        {
            throw new NotFoundException("Consignment contract not found");
        }

        List<ContractClause> clauses = DefaultContractTerms.Deserialize(contract.Terms).ToList();

        clauses.AddRange(request.AdditionalClauses);

        contract.Terms = DefaultContractTerms.Serialize(clauses);

        contract.Status = ConsignmentContractStatus.PendingLessorApproval;

        await context.SaveChangesAsync(cancellationToken);
    }
}
