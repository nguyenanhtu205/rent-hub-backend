using Application.Features.WorkHistories.Commands.CreateSurveySchedule;

namespace Web.Endpoints;

public class WorkHistory : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateSurveySchedule, "survey-schedule")
            .RequireAuthorization("Surveyor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("post");
    }

    [EndpointSummary("Create survey schedule")]
    [EndpointDescription("Create a new survey schedule for a property.")]
    public static async Task<IResult> CreateSurveySchedule(
        CreateSurveyScheduleCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
