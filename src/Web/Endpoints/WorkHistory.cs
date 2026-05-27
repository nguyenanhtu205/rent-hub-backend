using Application.Features.WorkHistories.Commands.CreateSurveySchedule;
using Application.Features.WorkHistories.Commands.CreateViewingSchedule;
using Application.Features.WorkHistories.Commands.UpdateSurveySchedule;
using Application.Features.WorkHistories.Commands.UpdateViewingSchedule;
using Application.Features.WorkHistories.Queries.GetAllWorkHistories;
using Application.Features.WorkHistories.Queries.GetSurveySchedule;
using Application.Features.WorkHistories.Queries.GetSurveyScheduleForLessor;
using Application.Features.WorkHistories.Queries.GetViewingScheduleForBroker;
using Application.Features.WorkHistories.Queries.GetViewingScheduleForRenter;
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

        groupBuilder.MapGet(GetAllWorkHistories, "all-work-histories")
            .RequireAuthorization("Manager")
            .Produces<List<AllWorkHistoryVm>>()
            .RequireRateLimiting("get");

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

        groupBuilder.MapPost(CreateViewingSchedule, "viewing-schedule")
            .RequireAuthorization("Renter")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status409Conflict)
            .RequireRateLimiting("post");

        groupBuilder.MapGet(GetViewingScheduleForRenter, "viewing-schedule/renter")
            .RequireAuthorization("Renter")
            .Produces<List<RenterViewingScheduleVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetViewingScheduleForBroker, "viewing-schedule/broker")
            .RequireAuthorization("Broker")
            .Produces<List<BrokerViewingScheduleVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapPatch(UpdateViewingSchedule, "viewing-schedule")
            .RequireAuthorization("Broker")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Create survey schedule")]
    [EndpointDescription("Create a new survey schedule for a property.")]
    public static async Task<IResult> CreateSurveySchedule(CreateSurveyScheduleCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Get all work histories")]
    public static async Task<IResult> GetAllWorkHistories(ISender sender, CancellationToken cancellationToken)
    {
        List<AllWorkHistoryVm> workHistories = await sender.Send(new GetAllWorkHistoriesQuery(), cancellationToken);
        return Results.Ok(workHistories);
    }

    [EndpointSummary("Get survey schedule for lessor")]
    [EndpointDescription("Get the survey schedule for the current lessor.")]
    public static async Task<IResult> GetSurveyScheduleForLessor(ISender sender, CancellationToken cancellationToken)
    {
        List<LessorSurveyScheduleVm> surveySchedules =
            await sender.Send(new GetSurveyScheduleForLessorQuery(), cancellationToken);
        return Results.Ok(surveySchedules);
    }

    [EndpointSummary("Get survey schedule")]
    [EndpointDescription("Get the survey schedule for the current surveyor.")]
    public static async Task<IResult> GetSurveySchedule(ISender sender, CancellationToken cancellationToken)
    {
        List<SurveyScheduleVm> surveySchedules = await sender.Send(new GetSurveyScheduleQuery(), cancellationToken);
        return Results.Ok(surveySchedules);
    }

    [EndpointSummary("Update survey schedule")]
    [EndpointDescription(
        "Update the survey schedule for a property. This will mark the work history as completed and update the property status based on whether the survey passed or not.")]
    public static async Task<IResult> UpdateSurveySchedule(UpdateSurveyScheduleCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Create viewing schedule")]
    [EndpointDescription("Create a new viewing schedule for a property, using by renter.")]
    public static async Task<IResult> CreateViewingSchedule(CreateViewingScheduleCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Get viewing schedule for renter")]
    public static async Task<IResult> GetViewingScheduleForRenter(ISender sender, CancellationToken cancellationToken)
    {
        List<RenterViewingScheduleVm> viewingSchedules =
            await sender.Send(new GetViewingScheduleForRenterQuery(), cancellationToken);
        return Results.Ok(viewingSchedules);
    }

    [EndpointSummary("Get viewing schedule for broker")]
    public static async Task<IResult> GetViewingScheduleForBroker(ISender sender, CancellationToken cancellationToken)
    {
        List<BrokerViewingScheduleVm> viewingSchedules =
            await sender.Send(new GetViewingScheduleForBrokerQuery(), cancellationToken);
        return Results.Ok(viewingSchedules);
    }

    [EndpointSummary("Update viewing schedule")]
    public static async Task<IResult> UpdateViewingSchedule(UpdateViewingScheduleCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
