namespace Application.Features.WorkHistories.Queries.GetViewingScheduleForBroker;

public record GetViewingScheduleForBrokerQuery : IRequest<List<BrokerViewingScheduleVm>>;

public class GetViewingScheduleForBrokerQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetViewingScheduleForBrokerQuery, List<BrokerViewingScheduleVm>>
{
    public async Task<List<BrokerViewingScheduleVm>> Handle(GetViewingScheduleForBrokerQuery request,
        CancellationToken cancellationToken)
    {
        List<WorkHistory> workHistories = await context.WorkHistories
            .AsNoTracking()
            .Include(wh => wh.Customer)
            .Where(wh => wh.StaffId == int.Parse(currentUser.Id!) && wh.Type == WorkHistoryType.BrokerTask)
            .OrderBy(wh => wh.Time)
            .ToListAsync(cancellationToken);

        if (workHistories.Count == 0)
        {
            return [];
        }

        List<int> propertyIds = workHistories
            .Select(wh => int.Parse(wh.Note.Split(':')[0]))
            .ToList();

        Dictionary<int, Property> properties = await context.Properties
            .AsNoTracking()
            .Where(p => propertyIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id, cancellationToken);

        return workHistories.Select(wh =>
        {
            string[] parts = wh.Note.Split(':');
            int propertyId = int.Parse(parts[0]);
            string isConfirm = parts[1];

            Property property = properties[propertyId];

            return new BrokerViewingScheduleVm
            {
                Id = wh.Id,
                Time = wh.Time,
                Status = wh.Status,
                CustomerName = wh.Customer!.Name,
                CustomerPhone = wh.Customer.Phone,
                PropertyType = property.Type,
                District = property.District,
                Address = property.Address,
                IsConfirm = isConfirm
            };
        }).ToList();
    }
}
