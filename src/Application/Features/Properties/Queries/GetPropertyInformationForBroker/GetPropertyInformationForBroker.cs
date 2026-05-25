namespace Application.Features.Properties.Queries.GetPropertyInformationForBroker;

public record GetPropertyInformationForBrokerQuery(int WorkHistoryId) : IRequest<PropertyInformationForBrokerVm>;

public class GetPropertyInformationForBrokerQueryHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IMapper mapper)
    : IRequestHandler<GetPropertyInformationForBrokerQuery, PropertyInformationForBrokerVm>
{
    public async Task<PropertyInformationForBrokerVm> Handle(GetPropertyInformationForBrokerQuery request,
        CancellationToken cancellationToken)
    {
        WorkHistory? workHistory = await context.WorkHistories.FindAsync([request.WorkHistoryId], cancellationToken);

        if (workHistory == null)
        {
            throw new NotFoundException("Work history not found.");
        }

        if (workHistory.StaffId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        int propertyId = int.Parse(workHistory.Note.Split(':')[0]);

        List<BrokerRoomDto> rooms = await context.Rooms
            .AsNoTracking()
            .Where(r => r.PropertyId == propertyId)
            .ProjectTo<BrokerRoomDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PropertyInformationForBrokerVm { Rooms = rooms };
    }
}
