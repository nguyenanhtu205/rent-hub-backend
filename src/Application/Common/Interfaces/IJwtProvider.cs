namespace Application.Common.Interfaces;

public interface IJwtProvider
{
    string Generate(int userId, int accountId, string role);
}
