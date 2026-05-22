namespace Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;

public record GetPropertyForCurrentSurveyorQuery : IRequest<List<PropertyForSurveyorVm>>;

public class GetPropertyForCurrentSurveyorQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUser currentUser)
    : IRequestHandler<GetPropertyForCurrentSurveyorQuery, List<PropertyForSurveyorVm>>
{
    public async Task<List<PropertyForSurveyorVm>> Handle(GetPropertyForCurrentSurveyorQuery request,
        CancellationToken cancellationToken)
    {
        string? staffId = currentUser.Id;
        string? role = currentUser.Role;

        if (staffId == null || role == null)
        {
            throw new UnauthorizedAccessException("User is not authenticated");
        }

        if (role is not "Surveyor")
        {
            throw new ForbiddenAccessException();
        }

        List<Property> properties = await context.Properties
            .AsNoTracking()
            .Include(p => p.Customer)
            .Where(p => p.StaffId == int.Parse(staffId) && p.Status == PropertyStatus.NotSurveyed)
            .ToListAsync(cancellationToken);

        if (properties.Count == 0)
        {
            return [];
        }

        List<int> propertyIds = properties.Select(p => p.Id).ToList();

        List<RoomDto> allRooms = await context.Rooms
            .AsNoTracking()
            .Where(r => propertyIds.Contains(r.PropertyId))
            .ProjectTo<RoomDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        List<DocumentDto> allDocuments = await context.PropertyDocuments
            .AsNoTracking()
            .Where(d => propertyIds.Contains(d.PropertyId))
            .ProjectTo<DocumentDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        Dictionary<int, List<RoomDto>> roomsByProperty = allRooms
            .GroupBy(r => r.PropertyId)
            .ToDictionary(g => g.Key, g => g.ToList());

        Dictionary<int, List<DocumentDto>> documentsByProperty = allDocuments
            .GroupBy(d => d.PropertyId)
            .ToDictionary(g => g.Key, g => g.ToList());

        Dictionary<int, PropertyDto> propertyDtoById = properties
            .ToDictionary(p => p.Id, mapper.Map<PropertyDto>);

        return properties.Select(p => new PropertyForSurveyorVm
        {
            Property = propertyDtoById[p.Id],
            Rooms = roomsByProperty.GetValueOrDefault(p.Id, []),
            Documents = documentsByProperty.GetValueOrDefault(p.Id, []),
            CustomerName = p.Customer!.Name,
            CustomerPhone = p.Customer!.Phone
        }).ToList();
    }
}
