using System.Text.Json;
using Application.Common.Exceptions;
using Application.Features.Properties.Commands.CreateProperty;
using Application.Features.Properties.Queries.GetAllProperties;
using Application.Features.Properties.Queries.GetPropertyForCurrentLessor;
using Application.Features.Properties.Queries.GetPropertyForCurrentSurveyor;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace Web.Endpoints;

public class Properties : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetAllProperties)
            .Produces<List<AllPropertyDto>>()
            .RequireRateLimiting("get");

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
    }

    [EndpointSummary("Get all properties")]
    public static async Task<IResult> GetAllProperties(ISender sender, CancellationToken cancellationToken)
    {
        List<AllPropertyDto> result = await sender.Send(new GetAllPropertiesQuery(), cancellationToken);
        return Results.Ok(result);
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
}
