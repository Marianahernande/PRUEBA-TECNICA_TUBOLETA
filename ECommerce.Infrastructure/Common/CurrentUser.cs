using System.Security.Claims;
using ECommerce.Application.Common;
using Microsoft.AspNetCore.Http;

namespace ECommerce.Infrastructure.Common;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _http;

    public CurrentUser(IHttpContextAccessor http) => _http = http;

    public int UserId
    {
        get
        {
            var str = _http.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.TryParse(str, out var id) ? id : 0;
        }
    }

    public string? Email => _http.HttpContext?.User.FindFirstValue(ClaimTypes.Email);
    public string? Role  => _http.HttpContext?.User.FindFirstValue(ClaimTypes.Role);
}
