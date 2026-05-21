using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services;

public class JwtProvider(IConfiguration configuration) : IJwtProvider
{
    public string Generate(int userId, int accountId, string role, string name, string phone, string email)
    {
        string? issuer = configuration["Jwt:Issuer"];
        string? audience = configuration["Jwt:Audience"];
        string? key = configuration["Jwt:SigningKey"];

        Claim[] claims =
        [
            new("sub", userId.ToString()),
            new("accountId", accountId.ToString()),
            new("role", role),
            new("name", name),
            new("phone", phone),
            new("email", email)
        ];

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key!));
        SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        JwtSecurityToken token = new(
            issuer,
            audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(120),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
