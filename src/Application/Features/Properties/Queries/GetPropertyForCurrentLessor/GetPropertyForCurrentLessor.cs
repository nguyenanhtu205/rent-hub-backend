namespace Application.Features.Properties.Queries.GetPropertyForCurrentLessor;

public record GetPropertyForCurrentLessorQuery : IRequest<PropertyVm>;

public class GetPropertyForCurrentLessorQueryHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IMapper mapper)
    : IRequestHandler<GetPropertyForCurrentLessorQuery, PropertyVm>
{
    public async Task<PropertyVm> Handle(GetPropertyForCurrentLessorQuery request, CancellationToken cancellationToken)
    {
        int lessorId = int.Parse(currentUser.Id!);

        List<PropertyDto> properties = await context.Properties
            .AsNoTracking()
            .Where(p => p.CustomerId == lessorId)
            .ProjectTo<PropertyDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PropertyVm { Properties = properties };
    }
}
