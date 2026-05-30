namespace Application.Features.Customers.Queries;

public record CustomerDto(string Name, string Phone, CustomerType Type);

public record GetCustomerQuery : IRequest<List<CustomerDto>>;

public class GetCustomerQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetCustomerQuery, List<CustomerDto>>
{
    public async Task<List<CustomerDto>> Handle(GetCustomerQuery request, CancellationToken cancellationToken)
    {
        return await context.Customers
            .Select(c => new CustomerDto(c.Name, c.Phone, c.Type))
            .ToListAsync(cancellationToken);
    }
}
