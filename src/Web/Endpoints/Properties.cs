using System.Text.Json;
using Application.Common.Exceptions;
using Application.Features.Properties.Commands.CreateProperty;
using Application.Features.Properties.Commands.UpdatePropertyForLessor;
using Application.Features.Properties.Queries.GetDetailInformation;
using Application.Features.Properties.Queries.GetDetailInformationForLessor;
using Application.Features.Properties.Queries.GetFeaturedProperties;
using Application.Features.Properties.Queries.GetPropertyByFilter;
using Application.Features.Properties.Queries.GetPropertyForCurrentLessor;
using Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;
using Application.Features.Properties.Queries.GetPropertyInformationForBroker;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class Properties : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetFeaturedProperties, "featured")
            .Produces<List<FeaturedPropertyVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetPropertyByFilter, "filter")
            .Produces<List<FilterPropertyVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetDetailInformation, "{propertyId:int}")
            .Produces<PropertyInformationVm>()
            .RequireAuthorization("Renter")
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetDetailInformationForLessor, "lessor/{propertyId:int}")
            .Produces<LessorDetailInformationVm>()
            .RequireAuthorization("Lessor")
            .RequireRateLimiting("get");

        groupBuilder.MapPatch(UpdatePropertyForLessor, "lessor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("Lessor")
            .RequireRateLimiting("put");

        groupBuilder.MapPost(CreateProperty)
            .Produces(StatusCodes.Status204NoContent)
            .RequireAuthorization("Lessor")
            .RequireRateLimiting("post")
            .DisableAntiforgery();

        groupBuilder.MapGet(GetPropertyForCurrentLessor, "for-lessor")
            .Produces<PropertyVm>()
            .RequireAuthorization("Lessor")
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetPropertyForCurrentSurveyor, "for-surveyor")
            .Produces<List<PropertyForSurveyorVm>>()
            .RequireAuthorization("Surveyor")
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetPropertyInformationForBroker, "for-broker/{workHistoryId:int}")
            .Produces<PropertyInformationForBrokerVm>()
            .RequireAuthorization("Broker")
            .RequireRateLimiting("get");
    }

    [EndpointSummary("Get featured properties")]
    public static async Task<IResult> GetFeaturedProperties(ISender sender, CancellationToken cancellationToken)
    {
        List<FeaturedPropertyVm> result = await sender.Send(new GetFeaturedPropertiesQuery(), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get properties by filter")]
    public static async Task<IResult> GetPropertyByFilter([FromQuery] string? q, [FromQuery] District? district,
        [FromQuery] PropertyType? type, ISender sender, CancellationToken cancellationToken)
    {
        GetPropertyByFilterQuery query = new(q, district, type);
        List<FilterPropertyVm> result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get property detail information")]
    public static async Task<IResult> GetDetailInformation(int propertyId, ISender sender,
        CancellationToken cancellationToken)
    {
        PropertyInformationVm result = await sender.Send(new GetDetailInformationQuery(propertyId), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get property detail information for lessor")]
    public static async Task<IResult> GetDetailInformationForLessor(int propertyId, ISender sender,
        CancellationToken cancellationToken)
    {
        LessorDetailInformationVm result =
            await sender.Send(new GetDetailInformationForLessorQuery(propertyId), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Update property information for lessor")]
    public static async Task<IResult> UpdatePropertyForLessor(UpdatePropertyForLessorCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Create property")]
    [EndpointDescription("Create a new property listing with images.")]
    public static async Task<IResult> CreateProperty(
        [FromForm] PropertyType propertyType,
        [FromForm] double area,
        [FromForm] string direction,
        [FromForm] int numOfRoom,
        [FromForm] string address,
        [FromForm] District district,
        [FromForm] string rooms,
        IFormFileCollection images,
        ISender sender,
        CancellationToken cancellationToken)
    {
        List<RoomRequest> roomRequests = JsonSerializer.Deserialize<List<RoomRequest>>(rooms)
                                         ?? throw new ValidationException([
                                             new ValidationFailure("rooms", "Invalid rooms format")
                                         ]);

        List<FileRequest> fileRequests = images
            .Select(f => new FileRequest(f.OpenReadStream(), f.FileName, f.ContentType))
            .ToList();

        CreatePropertyCommand command = new(
            propertyType,
            area,
            direction,
            numOfRoom,
            address,
            district,
            roomRequests,
            fileRequests);

        await sender.Send(command, cancellationToken);

        return Results.NoContent();
    }

    [EndpointSummary("Get properties for current surveyor")]
    public static async Task<IResult> GetPropertyForCurrentSurveyor(ISender sender, CancellationToken cancellationToken)
    {
        GetPropertyForCurrentSurveyorQuery query = new();
        List<PropertyForSurveyorVm> result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get properties for current lessor")]
    public static async Task<IResult> GetPropertyForCurrentLessor(ISender sender, CancellationToken cancellationToken)
    {
        GetPropertyForCurrentLessorQuery query = new();
        PropertyVm result = await sender.Send(query, cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get property information for broker")]
    public static async Task<IResult> GetPropertyInformationForBroker(int workHistoryId, ISender sender,
        CancellationToken cancellationToken)
    {
        PropertyInformationForBrokerVm result =
            await sender.Send(new GetPropertyInformationForBrokerQuery(workHistoryId), cancellationToken);
        return Results.Ok(result);
    }
}
