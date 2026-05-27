namespace Application.Features.Properties.Queries.GetDetailInformationForLessor;

public record GetDetailInformationForLessorQuery(int PropertyId) : IRequest<LessorDetailInformationVm>;

public class GetDetailInformationForLessorQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetDetailInformationForLessorQuery, LessorDetailInformationVm>
{
    public async Task<LessorDetailInformationVm> Handle(GetDetailInformationForLessorQuery request,
        CancellationToken cancellationToken)
    {
        Property? property = await context.Properties
            .Include(p => p.Staff)
            .Include(p => p.Rooms)
            .Include(p => p.PropertyDocuments)
            .Where(p => p.Id == request.PropertyId)
            .FirstOrDefaultAsync(cancellationToken);

        if (property == null)
        {
            throw new NotFoundException("Property not found");
        }

        if (property.CustomerId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        return new LessorDetailInformationVm
        {
            Type = property.Type,
            Area = property.Area,
            Direction = property.Direction,
            NumOfRoom = property.NumOfRoom,
            Address = property.Address,
            District = property.District,
            Status = property.Status,
            Price = property.Price,
            StaffName = property.Staff!.Name,
            StaffPhone = property.Staff.Phone,
            Rooms = property.Rooms.Select(r => new LessorRoomDto(r.Id, r.Name, r.Area, r.Price, r.Status, r.Notes))
                .ToList(),
            DocumentUrls = property.PropertyDocuments.Select(d => d.Url).ToList()
        };
    }
}
