namespace Application.Common.Interfaces;

public interface ICurrentUser
{
    string? Id { get; }

    string? AccountId { get; }

    string? Role { get; }
}
