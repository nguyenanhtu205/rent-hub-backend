using Application.Features.AuditLog.Queries;

namespace Web.Endpoints;

public class AuditLogs : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetAuditLogs, "audit-logs")
            .RequireAuthorization("Manager")
            .Produces<List<AuditLogDto>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetStatistics, "statistics")
            .RequireAuthorization("Manager")
            .Produces<StatisticsResult>()
            .RequireRateLimiting("get");
    }

    [EndpointSummary("Get audit logs")]
    public static async Task<IResult> GetAuditLogs(ISender sender, CancellationToken cancellationToken)
    {
        List<AuditLogDto> auditLogs = await sender.Send(new GetAuditLogQuery(), cancellationToken);
        return Results.Ok(auditLogs);
    }

    [EndpointSummary("Get statistics")]
    public static async Task<IResult> GetStatistics(ISender sender, CancellationToken cancellationToken)
    {
        StatisticsResult statistics = await sender.Send(new GetStatisticsQuery(), cancellationToken);
        return Results.Ok(statistics);
    }
}
