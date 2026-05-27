namespace Application.Features.WorkHistories.Queries.GetAllWorkHistories;

public record GetAllWorkHistoriesQuery : IRequest<List<AllWorkHistoryVm>>;

public class GetAllWorkHistoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAllWorkHistoriesQuery, List<AllWorkHistoryVm>>
{
    public async Task<List<AllWorkHistoryVm>> Handle(GetAllWorkHistoriesQuery request,
        CancellationToken cancellationToken)
    {
        List<AllWorkHistoryDto> workHistories = await context.WorkHistories
            .AsNoTracking()
            .Include(x => x.Staff)
            .Include(x => x.Customer)
            .ProjectTo<AllWorkHistoryDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        List<int> propertyIds = workHistories
            .Where(x => x.Type is WorkHistoryType.SurveyorTask or WorkHistoryType.BrokerTask)
            .Select(x => int.Parse(x.Note.Split(':')[0]))
            .Distinct()
            .ToList();

        Dictionary<int, Property> propertyMap = new();

        if (propertyIds.Count > 0)
        {
            propertyMap = await context.Properties
                .AsNoTracking()
                .Include(p => p.Customer)
                .Where(p => propertyIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, cancellationToken);
        }

        return workHistories
            .Select(x => MapToViewModel(x, propertyMap))
            .ToList();
    }

    private static AllWorkHistoryVm MapToViewModel(
        AllWorkHistoryDto x,
        Dictionary<int, Property> propertyMap)
    {
        return x.Type switch
        {
            WorkHistoryType.LegalTask => new AllWorkHistoryVm
            {
                Type = x.Type,
                Time = x.Time,
                Status = x.Status,
                StaffName = x.Staff!.Name,
                CustomerName = x.Customer!.Name,
                CustomerPhone = x.Customer!.Phone,
                AdditionalClauses = DefaultContractTerms.Deserialize(x.Note)
            },

            WorkHistoryType.SurveyorTask or WorkHistoryType.BrokerTask => BuildPropertyVm(x, propertyMap),

            _ => throw new ConflictException("Unknown work history type")
        };
    }

    private static AllWorkHistoryVm BuildPropertyVm(
        AllWorkHistoryDto x,
        Dictionary<int, Property> propertyMap)
    {
        int propertyId = int.Parse(x.Note.Split(':')[0]);

        if (!propertyMap.TryGetValue(propertyId, out Property? property))
        {
            throw new NotFoundException("Property not found");
        }

        return new AllWorkHistoryVm
        {
            Type = x.Type,
            Time = x.Time,
            Status = x.Status,
            StaffName = x.Staff!.Name,
            CustomerName = x.Customer!.Name,
            CustomerPhone = x.Customer!.Phone,
            Address = property.Address,
            PropertyType = property.Type,
            District = property.District,
            LessorName = property.Customer!.Name,
            LessorPhone = property.Customer!.Phone
        };
    }
}
