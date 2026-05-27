namespace Application.Features.Properties.Queries.GetFeaturedProperties;

public record GetFeaturedPropertiesQuery : IRequest<List<FeaturedPropertyVm>>;

public class GetFeaturedPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetFeaturedPropertiesQuery, List<FeaturedPropertyVm>>
{
    public async Task<List<FeaturedPropertyVm>> Handle(GetFeaturedPropertiesQuery request,
        CancellationToken cancellationToken)
    {
        List<string> notes = await context.WorkHistories
            .Where(w => w.Type == WorkHistoryType.BrokerTask)
            .Select(w => w.Note)
            .ToListAsync(cancellationToken);

        List<int> topPropertyIds = notes
            .Select(note => note.Split(':')[0])
            .Where(key => int.TryParse(key, out _))
            .GroupBy(key => key)
            .Select(g => new { PropertyId = int.Parse(g.Key), Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(3)
            .Select(x => x.PropertyId)
            .ToList();

        if (topPropertyIds.Count < 3)
        {
            List<int> fallback = await context.Properties
                .Where(p => !topPropertyIds.Contains(p.Id) && p.Status == PropertyStatus.Active)
                .OrderByDescending(p => p.Id)
                .Take(3 - topPropertyIds.Count)
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            topPropertyIds.AddRange(fallback);
        }

        if (topPropertyIds.Count == 0)
        {
            return [];
        }

        List<FeaturedPropertyDto> propertyDtos = await context.Properties
            .AsNoTracking()
            .Where(p => topPropertyIds.Contains(p.Id))
            .ProjectTo<FeaturedPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        Dictionary<int, string> propertyUrls = await context.PropertyDocuments
            .AsNoTracking()
            .Where(pd => topPropertyIds.Contains(pd.PropertyId))
            .GroupBy(pd => pd.PropertyId)
            .Select(g => new { PropertyId = g.Key, g.First().Url })
            .ToDictionaryAsync(pd => pd.PropertyId, pd => pd.Url, cancellationToken);

        return topPropertyIds
            .Select(id => propertyDtos.FirstOrDefault(p => p.Id == id))
            .Where(p => p != null && propertyUrls.ContainsKey(p.Id))
            .Select(p => new FeaturedPropertyVm { PropertyDto = p!, Url = propertyUrls[p!.Id] })
            .ToList();
    }
}
