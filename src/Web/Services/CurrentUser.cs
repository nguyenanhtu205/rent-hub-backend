using System.Security.Claims;
using Application.Common.Interfaces;

namespace Web.Services;

public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
{
    public string? Id => httpContextAccessor.HttpContext?.User.FindFirstValue("sub");

    public string? AccountId => httpContextAccessor.HttpContext?.User.FindFirstValue("accountId");

    public string? Role => httpContextAccessor.HttpContext?.User.FindFirstValue("role");
    
    public string? Name => httpContextAccessor.HttpContext?.User.FindFirstValue("name");
    
    public string? Phone => httpContextAccessor.HttpContext?.User.FindFirstValue("phone");
    
    public string? Email => httpContextAccessor.HttpContext?.User.FindFirstValue("email");
}
