using System.Security.Claims;
using Application.Common.Interfaces;

namespace Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? AccountId => httpContextAccessor.HttpContext?.User.FindFirstValue("accountId");
}
