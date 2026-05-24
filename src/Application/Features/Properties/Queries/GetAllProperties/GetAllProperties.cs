namespace Application.Features.Properties.Queries.GetAllProperties;

public record GetAllPropertiesQuery : IRequest<List<AllPropertyDto>>;

public class GetAllPropertiesQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetAllPropertiesQuery, List<AllPropertyDto>>
{
    public async Task<List<AllPropertyDto>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
    {
        return await context.Properties
            .AsNoTracking()
            .Where(p => p.Status == PropertyStatus.Active)
            .ProjectTo<AllPropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}
