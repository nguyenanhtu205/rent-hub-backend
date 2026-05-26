using Application.Features.RentalTransactions.Commands.CreateRentalTransaction;
using Application.Features.RentalTransactions.Commands.UpdateRentalTransactionForFinance;
using Application.Features.RentalTransactions.Commands.UpdateRentalTransactionForManager;
using Application.Features.RentalTransactions.Queries.GetRentalTransactionForBroker;
using Application.Features.RentalTransactions.Queries.GetRentalTransactionForFinance;
using Application.Features.RentalTransactions.Queries.GetRentalTransactionForManager;

namespace Web.Endpoints;

public class RentalTransactions : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapPost(CreateRentalTransaction)
            .RequireAuthorization("Broker")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("post");

        groupBuilder.MapGet(GetRentalTransactionsForBroker, "broker")
            .RequireAuthorization("Broker")
            .Produces<List<BrokerRentalTransactionVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetRentalTransactionsForManager, "manager")
            .RequireAuthorization("Manager")
            .Produces<List<ManagerRentalTransactionVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetRentalTransactionsForFinance, "finance")
            .RequireAuthorization("FinanceStaff")
            .Produces<List<FinanceRentalTransactionVm>>()
            .RequireRateLimiting("get");

        groupBuilder.MapPatch(UpdateRentalTransactionForFinance, "finance")
            .RequireAuthorization("FinanceStaff")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("put");

        groupBuilder.MapPatch(UpdateRentalTransactionForManager, "manager/{rentalTransactionId:int}")
            .RequireAuthorization("Manager")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("put");
    }

    [EndpointSummary("Create a new rental transaction")]
    public static async Task<IResult> CreateRentalTransaction(CreateRentalTransactionCommand command, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Get all rental transactions for broker")]
    public static async Task<IResult> GetRentalTransactionsForBroker(ISender sender,
        CancellationToken cancellationToken)
    {
        List<BrokerRentalTransactionVm> transactions =
            await sender.Send(new GetRentalTransactionForBrokerQuery(), cancellationToken);
        return Results.Ok(transactions);
    }

    [EndpointSummary("Get all rental transactions for manager")]
    public static async Task<IResult> GetRentalTransactionsForManager(ISender sender,
        CancellationToken cancellationToken)
    {
        List<ManagerRentalTransactionVm> transactions =
            await sender.Send(new GetRentalTransactionForManagerQuery(), cancellationToken);
        return Results.Ok(transactions);
    }

    [EndpointSummary("Get all rental transactions finance")]
    public static async Task<IResult> GetRentalTransactionsForFinance(ISender sender,
        CancellationToken cancellationToken)
    {
        List<FinanceRentalTransactionVm> transactions =
            await sender.Send(new GetRentalTransactionForFinanceQuery(), cancellationToken);
        return Results.Ok(transactions);
    }

    [EndpointSummary("Update the status of a rental transaction for finance")]
    public static async Task<IResult> UpdateRentalTransactionForFinance(
        UpdateRentalTransactionForFinanceCommand command, ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }

    [EndpointSummary("Update the status of a rental transaction to completed")]
    public static async Task<IResult> UpdateRentalTransactionForManager(int rentalTransactionId, ISender sender,
        CancellationToken cancellationToken)
    {
        await sender.Send(new UpdateRentalTransactionForManagerCommand(rentalTransactionId), cancellationToken);
        return Results.NoContent();
    }
}
