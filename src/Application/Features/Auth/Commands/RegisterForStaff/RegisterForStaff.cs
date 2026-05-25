namespace Application.Features.Auth.Commands.RegisterForStaff;

public record RegisterForStaffCommand(
    string Email,
    string Password,
    string Name,
    string Phone,
    StaffRole StaffRole
) : IRequest;

public class RegisterForStaffCommandHandler(IApplicationDbContext context, IPasswordHasher passwordHasher)
    : IRequestHandler<RegisterForStaffCommand>
{
    public async Task Handle(RegisterForStaffCommand request, CancellationToken cancellationToken)
    {
        bool emailExists = await context.Accounts
            .AnyAsync(x => x.Email == request.Email, cancellationToken);

        if (emailExists)
        {
            throw new ConflictException("Email already exists");
        }

        string hashedPassword = passwordHasher.Hash(request.Password);

        Account account = new()
        {
            Email = request.Email,
            Password = hashedPassword,
            Status = AccountStatus.Active,
            Role = AccountRole.Staff
        };

        context.Accounts.Add(account);

        Staff staff = new()
        {
            Name = request.Name,
            Phone = request.Phone,
            Role = request.StaffRole,
            ActiveWorkCount = 0,
            Account = account
        };

        context.Staffs.Add(staff);


        await context.SaveChangesAsync(cancellationToken);
    }
}
