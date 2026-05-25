namespace Application.Features.WorkHistories.Queries.GetViewingScheduleForRenter;

public record GetViewingScheduleForRenterQuery : IRequest<List<RenterViewingScheduleVm>>;

public class GetViewingScheduleForRenterQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetViewingScheduleForRenterQuery, List<RenterViewingScheduleVm>>
{
    public async Task<List<RenterViewingScheduleVm>> Handle(GetViewingScheduleForRenterQuery request,
        CancellationToken cancellationToken)
    {
        List<WorkHistory> workHistories = await context.WorkHistories
            .AsNoTracking()
            .Include(wh => wh.Staff)
            .Where(wh => wh.CustomerId == int.Parse(currentUser.Id!) && wh.Type == WorkHistoryType.BrokerTask)
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

            return new RenterViewingScheduleVm
            {
                Time = wh.Time,
                Status = wh.Status,
                StaffName = wh.Staff!.Name,
                StaffPhone = wh.Staff.Phone,
                PropertyType = property.Type,
                District = property.District,
                Address = property.Address,
                IsConfirm = isConfirm
            };
        }).ToList();
    }
}
