using Application.Features.FinancialTransactions.Commands.CreateDepositOffsetTransaction;
using Application.Features.FinancialTransactions.Queries.GetDetailTransaction;
using Application.Features.FinancialTransactions.Queries.GetFinancialTransaction;

namespace Web.Endpoints;

public class FinancialTransactions : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet(GetFinancialTransactions)
            .RequireAuthorization()
            .Produces<List<FinancialTransactionDto>>()
            .RequireRateLimiting("get");

        groupBuilder.MapGet(GetDetailTransaction, "/{transactionId}")
            .RequireAuthorization()
            .Produces<DetailTransactionVm>()
            .RequireRateLimiting("get");

        groupBuilder.MapPost(CreateDepositOffsetTransaction, "/deposit-offset")
            .RequireAuthorization("FinanceStaff")
            .Produces(StatusCodes.Status204NoContent)
            .RequireRateLimiting("post");
    }

    [EndpointSummary("Get financial transactions")]
    public static async Task<IResult> GetFinancialTransactions(ISender sender, CancellationToken cancellationToken)
    {
        List<FinancialTransactionDto>
            result = await sender.Send(new GetFinancialTransactionsQuery(), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Get detail transaction")]
    public static async Task<IResult> GetDetailTransaction(int transactionId, ISender sender,
        CancellationToken cancellationToken)
    {
        DetailTransactionVm result = await sender.Send(new GetDetailTransactionQuery(transactionId), cancellationToken);
        return Results.Ok(result);
    }

    [EndpointSummary("Create deposit offset transaction")]
    public static async Task<IResult> CreateDepositOffsetTransaction(CreateDepositOffsetTransactionCommand command,
        ISender sender, CancellationToken cancellationToken)
    {
        await sender.Send(command, cancellationToken);
        return Results.NoContent();
    }
}
