namespace Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;

public record GetPropertyForCurrentSurveyorQuery : IRequest<PropertyForSurveyorVm>;

public class GetPropertyForCurrentSurveyorQueryHandler(
    IApplicationDbContext context,
    IMapper mapper,
    ICurrentUser currentUser)
    : IRequestHandler<GetPropertyForCurrentSurveyorQuery, PropertyForSurveyorVm>
{
    public async Task<PropertyForSurveyorVm> Handle(GetPropertyForCurrentSurveyorQuery request,
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

        PropertyDto? propertyDto = await context.Properties
            .AsNoTracking()
            .Where(p => p.StaffId == int.Parse(staffId))
            .ProjectTo<PropertyDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (propertyDto == null)
        {
            return new PropertyForSurveyorVm { Property = null!, Rooms = [] };
        }

        List<RoomDto> rooms = await context.Rooms
            .AsNoTracking()
            .Where(r => r.PropertyId == propertyDto.Id)
            .ProjectTo<RoomDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        List<DocumentDto> documents = await context.PropertyDocuments
            .AsNoTracking()
            .Where(d => d.PropertyId == propertyDto.Id)
            .ProjectTo<DocumentDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PropertyForSurveyorVm { Property = propertyDto, Rooms = rooms, Documents = documents };
    }
}
