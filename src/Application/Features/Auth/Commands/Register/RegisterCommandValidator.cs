namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is not in the correct format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must have at least 6 characters");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(100).WithMessage("Name can't be more than 100 characters");

        RuleFor(x => x.Phone)
            .NotEmpty().WithMessage("Phone is required")
            .Matches(@"^\d{10,11}$").WithMessage("Phone must be 10-11 digits");

        RuleFor(x => x.CitizenId)
            .NotEmpty().WithMessage("Citizen ID is required")
            .Matches(@"^\d{12}$").WithMessage("Citizen ID must be 12 digits");

        RuleFor(x => x.CustomerType)
            .IsInEnum().WithMessage("Customer type is not valid");
    }
}
