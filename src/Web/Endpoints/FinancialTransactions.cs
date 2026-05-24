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
}
