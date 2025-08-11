using ECommerce.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Application.Auth;

public record LoginQuery(string Email, string Password) : IRequest<string>;

public class LoginHandler(
    IAppDbContext db,
    IPasswordHasher hasher,
    IJwtTokenService jwt
) : IRequestHandler<LoginQuery, string>
{
    public async Task<string> Handle(LoginQuery req, CancellationToken ct)
    {
        var email = req.Email.Trim().ToLowerInvariant();

        var user = await db.Users.FirstOrDefaultAsync(
            u => u.Email.ToLower() == email, ct);

        if (user is null) throw new UnauthorizedAccessException();

        if (!hasher.Verify(req.Password, user.PasswordHash))
            throw new UnauthorizedAccessException();

        return jwt.CreateToken(user);
    }
}
