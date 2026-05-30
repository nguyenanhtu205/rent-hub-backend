using Application.Features.AuditLog.Queries;

namespace Web.Endpoints;

public class AuditLogs : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetAuditLogs)
            .RequireAuthorization("Manager")
            .Produces<List<AuditLogDto>>()
            .RequireRateLimiting("get");
    }

    [EndpointSummary("Get audit logs")]
    public static async Task<IResult> GetAuditLogs(ISender sender, CancellationToken cancellationToken)
    {
        List<AuditLogDto> auditLogs = await sender.Send(new GetAuditLogQuery(), cancellationToken);
        return Results.Ok(auditLogs);
    }
}
