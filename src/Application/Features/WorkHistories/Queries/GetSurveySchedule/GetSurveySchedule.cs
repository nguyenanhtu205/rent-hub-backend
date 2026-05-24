namespace Application.Features.WorkHistories.Queries.GetSurveySchedule;

public record GetSurveyScheduleQuery : IRequest<List<SurveyScheduleVm>>;

public class GetSurveyScheduleQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetSurveyScheduleQuery, List<SurveyScheduleVm>>
{
    public async Task<List<SurveyScheduleVm>> Handle(GetSurveyScheduleQuery request,
        CancellationToken cancellationToken)
    {
        List<WorkHistory> surveySchedules = await context.WorkHistories
            .AsNoTracking()
            .Where(w => w.StaffId == int.Parse(currentUser.Id!))
            .Include(w => w.Customer)
            .ToListAsync(cancellationToken);

        if (surveySchedules.Count == 0)
        {
            return [];
        }

        List<int> propertyIds = surveySchedules.Select(s => int.Parse(s.Note)).ToList();

        Dictionary<string, string> addresses = await context.Properties
            .AsNoTracking()
            .Where(p => propertyIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id.ToString(), p => p.Address, cancellationToken);

        return surveySchedules.Select(s => new SurveyScheduleVm
        {
            Id = s.Id,
            CustomerName = s.Customer!.Name,
            CustomerPhone = s.Customer!.Phone,
            PropertyAddress = addresses[s.Note],
            Time = s.Time,
            Status = s.Status
        }).ToList();
    }
}
