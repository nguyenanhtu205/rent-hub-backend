using Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLessor;
using Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLessor;

namespace Web.Endpoints;

public class ConsignmentContracts : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetConsignmentContractForLessor, "lessor/{propertyId:int}")
            .RequireAuthorization("Lessor")
            .Produces<ConsignmentContractVm>()
            .RequireRateLimiting("get");

        groupBuilder.MapPatch(UpdateContractForLessor, "lessor")
            .RequireAuthorization("Lessor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Get consignment contract for lessor")]
    [EndpointDescription("Get consignment contract for lessor by property id")]
    public static async Task<IResult> GetConsignmentContractForLessor(
        int propertyId, ISender sender, CancellationToken cancellationToken)
    {
        ConsignmentContractVm contract =
            await sender.Send(new GetConsignmentContractsForLessorQuery(propertyId), cancellationToken);
        return Results.Ok(contract);
    }

    [EndpointSummary("Update consignment contract for lessor")]
    public static async Task<IResult> UpdateContractForLessor(
        UpdateContractForLessorCommand command, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
