namespace Application.Features.RentalTransactions.Commands.CreateRentalTransaction;

public record CreateRentalTransactionCommand(int WorkHistoryId, List<int> RoomIds, int PropertyId) : IRequest;

public class CreateRentalTransactionCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<CreateRentalTransactionCommand>
{
    public async Task Handle(CreateRentalTransactionCommand request, CancellationToken cancellationToken)
    {
        WorkHistory? workHistory = await context.WorkHistories.FindAsync([request.WorkHistoryId], cancellationToken);

        if (workHistory == null)
        {
            throw new NotFoundException("Work history not found.");
        }

        if (workHistory.StaffId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        if (workHistory.Type != WorkHistoryType.BrokerTask || workHistory.Status != WorkHistoryStatus.Completed)
        {
            throw new ConflictException("Work history must be a completed broker task.");
        }

        bool existingTransaction = await context.RentalTransactions
            .AnyAsync(rt => rt.StaffId == workHistory.StaffId
                            && rt.CustomerId == workHistory.CustomerId
                            && rt.PropertyId == request.PropertyId
                            && rt.Status == RentalTransactionStatus.PendingFinance, cancellationToken);

        if (existingTransaction)
        {
            throw new ConflictException("A rental transaction with the same details already exists.");
        }

        double totalPrice = await context.Rooms
            .Where(r => request.RoomIds.Contains(r.Id))
            .SumAsync(r => r.Price, cancellationToken);

        RentalTransaction rentalTransaction = new()
        {
            Price = totalPrice,
            CommissionRate = DefaultContractTerms.CommissionRate,
            ClosedTime = DateTimeOffset.UtcNow,
            Status = RentalTransactionStatus.PendingFinance,
            StaffId = workHistory.StaffId,
            CustomerId = workHistory.CustomerId,
            PropertyId = request.PropertyId,
            CommissionAmount = totalPrice * DefaultContractTerms.CommissionRate
        };

        context.RentalTransactions.Add(rentalTransaction);

        List<RoomRentalTransaction> roomRentalTransactions = request.RoomIds.Select(id =>
            new RoomRentalTransaction { RoomId = id, RentalTransaction = rentalTransaction }).ToList();

        context.RoomRentalTransactions.AddRange(roomRentalTransactions);

        await context.Rooms.Where(r => request.RoomIds.Contains(r.Id))
            .ExecuteUpdateAsync(x => x.SetProperty(r
                => r.Status, RoomStatus.Rented), cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }
}
