using Application.Features.WorkHistories.Commands.CreateSurveySchedule;
using Application.Features.WorkHistories.Queries.GetSurveySchedule;
using Application.Features.WorkHistories.Queries.GetSurveyScheduleForLessor;
using Application.Features.WorkHistories.Queries.UpdateSurveySchedule;
using SurveyScheduleVm = Application.Features.WorkHistories.Queries.GetSurveySchedule.SurveyScheduleVm;

namespace Web.Endpoints;

public class WorkHistory : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateSurveySchedule, "survey-schedule")
            .RequireAuthorization("Surveyor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("post");

        groupBuilder.MapGet(GetSurveyScheduleForLessor, "survey-schedule/lessor")
            .RequireAuthorization("Lessor")
            .Produces<List<LessorSurveyScheduleVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetSurveySchedule, "survey-schedule")
            .RequireAuthorization("Surveyor")
            .Produces<List<SurveyScheduleVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapPatch(UpdateSurveySchedule, "survey-schedule")
            .RequireAuthorization("Surveyor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("put");
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

    [EndpointSummary("Get survey schedule for lessor")]
    [EndpointDescription("Get the survey schedule for the current lessor.")]
    public static async Task<IResult> GetSurveyScheduleForLessor(
        ISender sender,
        CancellationToken cancellationToken)
    {
        List<LessorSurveyScheduleVm> surveySchedules =
            await sender.Send(new GetSurveyScheduleForLessorQuery(), cancellationToken);
        return Results.Ok(surveySchedules);
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

    [EndpointSummary("Update survey schedule")]
    [EndpointDescription(
        "Update the survey schedule for a property. This will mark the work history as completed and update the property status based on whether the survey passed or not.")]
    public static async Task<IResult> UpdateSurveySchedule(
        UpdateSurveyScheduleCommand command,
        ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
