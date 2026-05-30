using Application.Features.Customers.Queries;

namespace Web.Endpoints;

public class Customers : IEndpointGroup
{
    public static void Map(RouteGroupBuilder groupBuilder)
    {
        groupBuilder.MapGet("/", GetCustomers)
            .RequireAuthorization("Manager")
            .Produces<List<CustomerDto>>()
            .RequireRateLimiting("get");
    }

    [EndpointSummary("Get all customers")]
    public static async Task<IResult> GetCustomers(ISender sender, CancellationToken cancellationToken)
    {
        List<CustomerDto> customers = await sender.Send(new GetCustomerQuery(), cancellationToken);
        return Results.Ok(customers);
    }
}
