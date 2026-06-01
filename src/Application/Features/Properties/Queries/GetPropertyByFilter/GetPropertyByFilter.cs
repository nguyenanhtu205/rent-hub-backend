namespace Application.Features.Properties.Queries.GetPropertyByFilter;

public record GetPropertyByFilterQuery(string? Query, District? District, PropertyType? PropertyType)
    : IRequest<List<FilterPropertyVm>>;

public class GetPropertyByFilterQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetPropertyByFilterQuery, List<FilterPropertyVm>>
{
    public async Task<List<FilterPropertyVm>> Handle(GetPropertyByFilterQuery request,
        CancellationToken cancellationToken)
    {
        IQueryable<Property> propertiesQuery = context.Properties
            .AsNoTracking()
            .Where(p => p.Status == PropertyStatus.Active);

        if (request.District.HasValue)
        {
            propertiesQuery = propertiesQuery.Where(p => p.District == request.District.Value);
        }

        if (request.PropertyType.HasValue)
        {
            propertiesQuery = propertiesQuery.Where(p => p.Type == request.PropertyType.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Query))
        {
            propertiesQuery = propertiesQuery.Where(p =>
                p.Address.Contains(request.Query)); 
        }

        List<FilterPropertyDto> propertyDto = await propertiesQuery
            .ProjectTo<FilterPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (propertyDto.Count == 0)
        {
            return [];
        }

        List<int> ids = propertyDto.Select(p => p.Id).ToList();

        Dictionary<int, string> propertyUrls = await context.PropertyDocuments
            .AsNoTracking()
            .Where(pd => ids.Contains(pd.PropertyId))
            .GroupBy(pd => pd.PropertyId)
            .Select(g => new { PropertyId = g.Key, g.First().Url })
            .ToDictionaryAsync(pd => pd.PropertyId, pd => pd.Url, cancellationToken);

        return propertyDto
            .Where(p => propertyUrls.ContainsKey(p.Id))
            .Select(p => new FilterPropertyVm { PropertyDto = p, Url = propertyUrls[p.Id] })
            .ToList();
    }
}
