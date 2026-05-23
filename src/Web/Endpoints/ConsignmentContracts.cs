using Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLessor;

namespace Web.Endpoints;

public class ConsignmentContracts : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetConsignmentContractForLessor, "{propertyId:int}")
            .RequireAuthorization("Lessor")
            .Produces<ConsignmentContractVm>()
            .RequireRateLimiting("get");
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
}
