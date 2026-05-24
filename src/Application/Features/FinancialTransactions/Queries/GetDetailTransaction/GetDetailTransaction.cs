namespace Application.Features.FinancialTransactions.Queries.GetDetailTransaction;

public record GetDetailTransactionQuery(int TransactionId) : IRequest<DetailTransactionVm>;

public class GetDetailTransactionQueryHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<GetDetailTransactionQuery, DetailTransactionVm>
{
    public async Task<DetailTransactionVm> Handle(GetDetailTransactionQuery request,
        CancellationToken cancellationToken)
    {
        string role = currentUser.Role!;

        if (role != nameof(StaffRole.FinanceStaff) && role != nameof(StaffRole.Manager))
        {
            throw new ForbiddenAccessException();
        }

        FinancialTransaction? transaction = await context.FinancialTransactions
            .AsNoTracking()
            .Where(t => t.Id == request.TransactionId)
            .Include(t => t.Staff)
            .FirstOrDefaultAsync(cancellationToken);

        if (transaction == null)
        {
            throw new NotFoundException("Transaction not found");
        }

        PropertyDto? propertyDto;
        RentalTransactionDto? rentalTransactionDto;
        string customerName;
        string customerPhone;

        if (transaction.RefType == RefType.RentalTransaction)
        {
            RentalTransaction? rentalTransaction = await context.RentalTransactions
                .AsNoTracking()
                .Where(rt => rt.Id == transaction.RefId)
                .Include(rt => rt.Customer)
                .FirstOrDefaultAsync(cancellationToken);

            rentalTransactionDto = new RentalTransactionDto
            {
                ClosedTime = rentalTransaction!.ClosedTime,
                CommissionRate = rentalTransaction.CommissionRate,
                Price = rentalTransaction.Price
            };

            customerName = rentalTransaction.Customer!.Name;
            customerPhone = rentalTransaction.Customer.Phone;

            propertyDto = null;
        }
        else
        {
            ConsignmentContract? contract = await context.ConsignmentContracts
                .AsNoTracking()
                .Where(c => c.Id == transaction.RefId)
                .FirstOrDefaultAsync(cancellationToken);

            Property? property = await context.Properties
                .AsNoTracking()
                .Where(p => p.Id == contract!.PropertyId)
                .Include(p => p.Customer)
                .FirstOrDefaultAsync(cancellationToken);

            propertyDto = new PropertyDto { Type = property!.Type, Address = property.Address };

            customerName = property.Customer!.Name;
            customerPhone = property.Customer!.Phone;

            rentalTransactionDto = null;
        }

        return new DetailTransactionVm
        {
            Type = transaction.Type,
            Amount = transaction.Amount,
            Method = transaction.Method,
            Date = transaction.Date,
            RefType = transaction.RefType,
            CustomerName = customerName,
            CustomerPhone = customerPhone,
            StaffName = transaction.Staff!.Name,
            Property = propertyDto,
            RentalTransaction = rentalTransactionDto
        };
    }
}
