namespace Application.Features.Properties.Commands.CreateProperty;

public record FileRequest(Stream Content, string FileName, string ContentType);

public record RoomRequest(string Name, double Area, double Price, RoomStatus RoomStatus, string Notes);

public record CreatePropertyCommand(
    PropertyType PropertyType,
    double Area,
    string Direction,
    int NumOfRoom,
    string Address,
    District District,
    List<RoomRequest> Rooms,
    List<FileRequest> Images
) : IRequest;

public class CreatePropertyCommandHandler(
    IApplicationDbContext context,
    IStorageService storageService,
    ICurrentUser currentUser)
    : IRequestHandler<CreatePropertyCommand>
{
    public async Task Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
    {
        string? userRole = currentUser.Role;
        string? userId = currentUser.Id;
        string? accountId = currentUser.AccountId;

        if (userRole == null || userId == null || accountId == null)
        {
            throw new UnauthorizedAccessException("User is not logged in");
        }

        if (userRole != nameof(CustomerType.Lessor))
        {
            throw new ForbiddenAccessException();
        }

        int staffId = await context.StaffWorkingAreas
            .Where(x =>
                x.District == request.District &&
                x.Role == StaffRole.Surveyor)
            .Select(x => x.StaffId)
            .Distinct()
            .Join(
                context.Staffs,
                id => id,
                staff => staff.Id,
                (id, staff) => staff)
            .OrderBy(s => s.ActiveWorkCount)
            .Select(s => s.Id)
            .FirstAsync(cancellationToken);


        Property newProperty = new()
        {
            Type = request.PropertyType,
            Area = request.Area,
            Direction = request.Direction,
            NumOfRoom = request.NumOfRoom,
            Address = request.Address,
            District = request.District,
            Status = PropertyStatus.NotSurveyed,
            Price = request.Rooms.Count > 0 ? request.Rooms.Min(r => r.Price) : 0,
            StaffId = staffId,
            CustomerId = int.Parse(userId)
        };

        context.Properties.Add(newProperty);

        List<(string Name, string Url)> uploadedFiles = [];

        foreach (FileRequest file in request.Images)
        {
            string url =
                await storageService.UploadAsync(file.Content, file.FileName, file.ContentType, cancellationToken);
            uploadedFiles.Add((file.FileName, url));
        }

        IEnumerable<PropertyDocument> propertyDocuments = uploadedFiles.Select(f =>
            new PropertyDocument
            {
                Name = f.Name, Type = PropertyDocumentType.Image, Url = f.Url, Property = newProperty
            });

        context.PropertyDocuments.AddRange(propertyDocuments);

        IEnumerable<Room> rooms = request.Rooms.Select(r =>
            new Room
            {
                Name = r.Name,
                Area = r.Area,
                Price = r.Price,
                Status = r.RoomStatus,
                Notes = r.Notes,
                Property = newProperty
            });

        context.Rooms.AddRange(rooms);

        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            foreach ((string Name, string Url) file in uploadedFiles)
            {
                await storageService.DeleteAsync(file.Url);
            }

            throw;
        }
    }
}
