using ECommerce.Domain.Entities;

namespace ECommerce.Application.Auth;

public interface IJwtTokenService
{
    string CreateToken(User user);
}
