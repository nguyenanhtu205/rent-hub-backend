using Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLegalStaff;
using Application.Features.ConsignmentContracts.Commands.UpdateConsignmentContractForLessor;
using Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForFinanceStaff;
using Application.Features.ConsignmentContracts.Queries.GetConsignmentContractForLegalStaff;
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

        groupBuilder.MapGet(GetContractsForLegalStaff, "legal-staff")
            .RequireAuthorization("LegalStaff")
            .Produces<List<LegalContractVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetContractsForFinanceStaff, "finance-staff")
            .RequireAuthorization("FinanceStaff")
            .Produces<List<FinanceContractVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapPatch(UpdateContractForLessor, "lessor")
            .RequireAuthorization("Lessor")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("put");

        groupBuilder.MapPatch(UpdateContractForLegalStaff, "legal-staff")
            .RequireAuthorization("LegalStaff")
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

    [EndpointSummary("Get consignment contracts for legal staff")]
    public static async Task<IResult> GetContractsForLegalStaff(ISender sender, CancellationToken cancellationToken)
    {
        List<LegalContractVm> contracts = await sender.Send(new GetContractForLegalStaffQuery(), cancellationToken);
        return Results.Ok(contracts);
    }

    [EndpointSummary("Update consignment contract for legal staff")]
    public static async Task<IResult> UpdateContractForLegalStaff(
        UpdateContractForLegalStaffCommand command, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Get consignment contracts for finance staff")]
    public static async Task<IResult> GetContractsForFinanceStaff(ISender sender, CancellationToken cancellationToken)
    {
        List<FinanceContractVm> contracts = await sender.Send(new GetContractForFinanceStaffQuery(), cancellationToken);
        return Results.Ok(contracts);
    }
}
