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
    }

    public static async Task<IResult> GetFinancialTransactions(ISender sender, CancellationToken cancellationToken)
    {
        List<FinancialTransactionDto>
            result = await sender.Send(new GetFinancialTransactionsQuery(), cancellationToken);
        return Results.Ok(result);
    }
}
