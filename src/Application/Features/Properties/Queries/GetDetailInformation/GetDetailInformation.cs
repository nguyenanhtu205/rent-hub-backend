namespace Application.Features.Properties.Queries.GetDetailInformation;

public record GetDetailInformationQuery(int PropertyId) : IRequest<PropertyInformationVm>;

public class GetDetailInformationQueryHandler(IApplicationDbContext context, IMapper mapper)
    : IRequestHandler<GetDetailInformationQuery, PropertyInformationVm>
{
    public async Task<PropertyInformationVm> Handle(GetDetailInformationQuery request,
        CancellationToken cancellationToken)
    {
        PropertyInformationDto? propertyDto = await context.Properties
            .AsNoTracking()
            .Where(p => p.Id == request.PropertyId && p.Status == PropertyStatus.Active)
            .Include(p => p.Staff)
            .ProjectTo<PropertyInformationDto>(mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);

        if (propertyDto == null)
        {
            throw new NotFoundException("Property not found");
        }

        List<string> urls = await context.PropertyDocuments
            .AsNoTracking()
            .Where(d => d.PropertyId == request.PropertyId)
            .Select(d => d.Url)
            .ToListAsync(cancellationToken);

        List<RoomDto> rooms = await context.Rooms
            .AsNoTracking()
            .Where(r => r.PropertyId == request.PropertyId)
            .Select(r => new RoomDto(r.Name, r.Area, r.Price, r.Status, r.Notes))
            .ToListAsync(cancellationToken);

        return new PropertyInformationVm
        {
            Id = propertyDto.Id,
            Type = propertyDto.Type,
            Area = propertyDto.Area,
            Direction = propertyDto.Direction,
            NumOfRoom = propertyDto.NumOfRoom,
            Address = propertyDto.Address,
            District = propertyDto.District,
            Price = propertyDto.Price,
            StaffName = propertyDto.Staff!.Name,
            StaffPhone = propertyDto.Staff.Phone,
            Rooms = rooms,
            DocumentUrls = urls
        };
    }
}
