using System.Text.Json;
using MediatR.Pipeline;

namespace Application.Common.Behaviours;

public class LoggingBehaviour<TRequest>(
    ILogger<TRequest> logger,
    ICurrentUser currentUser,
    IApplicationDbContext context)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        string requestName = typeof(TRequest).Name;
        string userId = currentUser.Id ?? string.Empty;

        if (logger.IsEnabled(LogLevel.Information))
        {
            logger.LogInformation("Request: {Name} {@UserId} {@Request}",
                requestName, userId, request);
        }

        AuditLog auditLog = new()
        {
            RequestName = requestName,
            UserId = string.IsNullOrEmpty(userId) ? null : userId,
            RequestPayload = JsonSerializer.Serialize(request)
        };

        context.AuditLogs.Add(auditLog);
        await context.SaveChangesAsync(cancellationToken);
    }
}
