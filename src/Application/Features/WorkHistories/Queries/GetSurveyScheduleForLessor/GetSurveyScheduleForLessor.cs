namespace Application.Features.WorkHistories.Queries.GetSurveyScheduleForLessor;

public record GetSurveyScheduleForLessorQuery : IRequest<List<LessorSurveyScheduleVm>>;

public class GetSurveyScheduleForLessorQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetSurveyScheduleForLessorQuery, List<LessorSurveyScheduleVm>>
{
    public async Task<List<LessorSurveyScheduleVm>> Handle(GetSurveyScheduleForLessorQuery request,
        CancellationToken cancellationToken)
    {
        List<WorkHistory> surveySchedules = await context.WorkHistories
            .Where(w => w.CustomerId == int.Parse(currentUser.Id!))
            .Include(w => w.Staff)
            .ToListAsync(cancellationToken);

        if (surveySchedules.Count == 0)
        {
            return [];
        }
        
        List<int> propertyIds = surveySchedules.Select(s => int.Parse(s.Note)).ToList();
        
        Dictionary<string, string> addresses = await context.Properties
            .Where(p => propertyIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id.ToString(), p => p.Address, cancellationToken);
        
        Dictionary<string, PropertyType> types = await context.Properties
            .Where(p => propertyIds.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id.ToString(), p => p.Type, cancellationToken);
        
        return surveySchedules.Select(s => new LessorSurveyScheduleVm
        {
            PropertyAddress = addresses[s.Note],
            Type = types[s.Note],
            Time = s.Time,
            Status = s.Status,
            SurveyorName = s.Staff!.Name,
            Phone = s.Staff!.Phone
        }).ToList();
    }
}
