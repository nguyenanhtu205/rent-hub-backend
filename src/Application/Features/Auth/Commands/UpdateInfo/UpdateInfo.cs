namespace Application.Features.Auth.Commands.UpdateInfo;

public record UpdateInfoCommand(string Email, string Password) : IRequest;

public class UpdateInfoCommandHandler(
    IApplicationDbContext context,
    ICurrentUser currentUser,
    IPasswordHasher passwordHasher) : IRequestHandler<UpdateInfoCommand>
{
    public async Task Handle(UpdateInfoCommand request, CancellationToken cancellationToken)
    {
        int accountId = int.Parse(currentUser.AccountId!);

        string hashedPassword = passwordHasher.Hash(request.Password);

        await context.Accounts
            .Where(x => x.Id == accountId)
            .ExecuteUpdateAsync(x => x
                .SetProperty(a => a.Email, request.Email)
                .SetProperty(a => a.Password, hashedPassword), cancellationToken);
    }
}
