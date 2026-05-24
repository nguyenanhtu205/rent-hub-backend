namespace Application.Features.Properties.Queries.GetAllProperties;

public record GetAllPropertiesQuery : IRequest<List<AllPropertyVm>>;

public class GetAllPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAllPropertiesQuery, List<AllPropertyVm>>
{
    public async Task<List<AllPropertyVm>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
    {
        List<AllPropertyDto> propertyDto = await context.Properties
            .AsNoTracking()
            .Where(p => p.Status == PropertyStatus.Active)
            .ProjectTo<AllPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        if (propertyDto.Count == 0)
        {
            return [];
        }

        Dictionary<int, string> propertyUrls = await context.PropertyDocuments
            .AsNoTracking()
            .Where(pd => propertyDto.Select(p => p.Id).Contains(pd.PropertyId))
            .GroupBy(pd => pd.PropertyId)
            .Select(g => new { PropertyId = g.Key, g.First().Url })
            .ToDictionaryAsync(pd => pd.PropertyId, pd => pd.Url, cancellationToken);

        return propertyDto.Select(p => new AllPropertyVm { PropertyDto = p, Url = propertyUrls[p.Id] }).ToList();
    }
}
