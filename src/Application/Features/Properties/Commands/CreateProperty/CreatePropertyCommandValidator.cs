namespace Application.Features.Properties.Commands.CreateProperty;

public class CreatePropertyCommandValidator : AbstractValidator<CreatePropertyCommand>
{
    public CreatePropertyCommandValidator()
    {
        RuleFor(x => x.Area)
            .GreaterThan(0).WithMessage("Area must be greater than 0");

        RuleFor(x => x.Direction)
            .NotEmpty().WithMessage("Direction is required")
            .MaximumLength(150).WithMessage("Direction must not exceed 150 characters");

        RuleFor(x => x.NumOfRoom)
            .GreaterThan(0).WithMessage("Number of rooms must be greater than 0");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required");

        RuleFor(x => x.District)
            .IsInEnum().WithMessage("Invalid district");

        RuleFor(x => x.PropertyType)
            .IsInEnum().WithMessage("Invalid property type");

        RuleFor(x => x.Rooms)
            .NotEmpty().WithMessage("At least one room is required")
            .Must((cmd, rooms) => rooms.Count == cmd.NumOfRoom)
            .WithMessage("Room count does not match NumOfRoom");

        RuleForEach(x => x.Rooms).ChildRules(room =>
        {
            room.RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Room name is required")
                .MaximumLength(100).WithMessage("Room name must not exceed 100 characters");

            room.RuleFor(r => r.Area)
                .GreaterThan(0).WithMessage("Room area must be greater than 0");

            room.RuleFor(r => r.Price)
                .GreaterThanOrEqualTo(0).WithMessage("Room price must not be negative");

            room.RuleFor(r => r.RoomStatus)
                .IsInEnum().WithMessage("Invalid room status");
        });

        RuleFor(x => x.Images)
            .NotEmpty().WithMessage("At least one image is required");

        RuleForEach(x => x.Images).ChildRules(image =>
        {
            image.RuleFor(f => f.FileName)
                .NotEmpty().WithMessage("File name is required")
                .MaximumLength(255).WithMessage("File name must not exceed 255 characters");

            image.RuleFor(f => f.ContentType)
                .NotEmpty().WithMessage("Content type is required")
                .Must(ct => ct.StartsWith("image/"))
                .WithMessage("Only image files are allowed");

            image.RuleFor(f => f.Content)
                .NotNull().WithMessage("File content is required");
        });
    }
}
