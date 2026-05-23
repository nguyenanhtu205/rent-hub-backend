namespace Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLessor;

public class ConsignmentContractDto
{
    public DateTimeOffset? SigningDate { get; init; }

    public double RemainingDeposit { get; init; }

    public required ConsignmentContractStatus Status { get; init; }

    public required string Terms { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ConsignmentContract, ConsignmentContractDto>();
        }
    }
}
