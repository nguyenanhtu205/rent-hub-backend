namespace Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLegalStaff;

public record UpdateContractForLegalStaffCommand(int ContractId, List<ContractClause> AdditionalClauses) : IRequest;

public class UpdateContractForLegalStaffCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
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

        Property? property = await context.Properties
            .AsNoTracking()
            .Where(p => p.Id == contract.PropertyId)
            .FirstOrDefaultAsync(cancellationToken);

        context.WorkHistories.Add(new WorkHistory
        {
            Type = WorkHistoryType.LegalTask,
            Time = DateTimeOffset.UtcNow,
            Note = DefaultContractTerms.Serialize(request.AdditionalClauses),
            Status = WorkHistoryStatus.Completed,
            StaffId = int.Parse(currentUser.Id!),
            CustomerId = property!.CustomerId
        });

        await context.SaveChangesAsync(cancellationToken);
    }
}
