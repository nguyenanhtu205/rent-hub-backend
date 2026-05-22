using Application.Features.WorkHistories.Commands.CreateSurveySchedule;
using Application.Features.WorkHistories.Queries.GetSurveySchedule;

namespace Web.Endpoints;

public class WorkHistory : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateSurveySchedule, "survey-schedule")
            .RequireAuthorization("Surveyor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("post");

        groupBuilder.MapGet(GetSurveySchedule, "survey-schedule")
            .RequireAuthorization("Surveyor")
            .Produces<List<SurveyScheduleVm>>()
            .RequireRateLimiting("get");
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

    [EndpointSummary("Get survey schedule")]
    [EndpointDescription("Get the survey schedule for the current surveyor.")]
    public static async Task<IResult> GetSurveySchedule(
        ISender sender,
        CancellationToken cancellationToken)
    {
        List<SurveyScheduleVm> surveySchedules = await sender.Send(new GetSurveyScheduleQuery(), cancellationToken);
        return Results.Ok(surveySchedules);
    }
}
