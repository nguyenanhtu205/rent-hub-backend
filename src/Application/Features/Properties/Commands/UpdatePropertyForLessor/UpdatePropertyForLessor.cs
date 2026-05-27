namespace Application.Features.Properties.Commands.UpdatePropertyForLessor;

public record UpdatePropertyForLessorCommand(List<int> RoomIds) : IRequest;

public class UpdatePropertyForLessorCommandHandler(IApplicationDbContext context, ICurrentUser currentUser)
    : IRequestHandler<UpdatePropertyForLessorCommand>
{
    public async Task Handle(UpdatePropertyForLessorCommand request, CancellationToken cancellationToken)
    {
        List<Room> rooms = await context.Rooms
            .Where(r => request.RoomIds.Contains(r.Id))
            .Include(r => r.Property)
            .ToListAsync(cancellationToken);

        if (rooms.Count == 0)
        {
            throw new NotFoundException("No rooms found with the provided IDs.");
        }

        if (rooms[0].Property!.CustomerId != int.Parse(currentUser.Id!))
        {
            throw new ForbiddenAccessException();
        }

        foreach (Room room in rooms)
        {
            room.Status = RoomStatus.Available;
        }

        await context.SaveChangesAsync(cancellationToken);
    }
}
