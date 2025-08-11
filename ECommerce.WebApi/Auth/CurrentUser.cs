using System.Security.Claims;
using ECommerce.Application.Common;

namespace ECommerce.WebApi.Auth;

public class CurrentUser(IHttpContextAccessor acc) : ICurrentUser
{
    public bool IsAuthenticated => acc.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public int UserId
    {
        get
        {
            var u = acc.HttpContext?.User;
            var sub = u?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                   ?? u?.FindFirst("sub")?.Value
                   ?? throw new UnauthorizedAccessException("No autenticado.");
            return int.Parse(sub);
        }
    }
}
