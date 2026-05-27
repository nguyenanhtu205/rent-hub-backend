namespace Application.Features.WorkHistories.Queries.GetAllWorkHistories;

public class AllWorkHistoryDto
{
    public required WorkHistoryType Type { get; init; }

    public DateTimeOffset Time { get; init; }

    public required string Note { get; set; }

    public required WorkHistoryStatus Status { get; init; }

    public Staff? Staff { get; init; }

    public Customer? Customer { get; init; }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<WorkHistory, AllWorkHistoryDto>();
        }
    }
}
