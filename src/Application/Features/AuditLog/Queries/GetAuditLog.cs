namespace Application.Features.AuditLog.Queries;

public record AuditLogDto(string RequestName, string? UserId, DateTimeOffset CreatedAt);

public record GetAuditLogQuery : IRequest<List<AuditLogDto>>;

public class GetAuditLogQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetAuditLogQuery, List<AuditLogDto>>
{
    public async Task<List<AuditLogDto>> Handle(GetAuditLogQuery request, CancellationToken cancellationToken)
    {
        return await context.AuditLogs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new AuditLogDto(
                x.RequestName,
                x.UserId,
                x.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}
