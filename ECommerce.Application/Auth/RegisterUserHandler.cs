using MediatR;
using Microsoft.EntityFrameworkCore;
using ECommerce.Application.Abstractions;
using ECommerce.Application.Notifications;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Auth;

public class RegisterUserHandler(
    IAppDbContext db,
    IPasswordHasher hasher,
    IJwtTokenService jwt,
    INotificationService notifications
) : IRequestHandler<RegisterUserCommand, string>
{
    public async Task<string> Handle(RegisterUserCommand req, CancellationToken ct)
    {
        // 1) Normaliza rol (seguridad: default User)
        var role = string.IsNullOrWhiteSpace(req.Role) ? "User" : req.Role.Trim();
        role = char.ToUpper(role[0]) + role[1..].ToLower();

        // 2) Unicidad por email
        var email = req.Email.Trim().ToLowerInvariant();
        var exists = await db.Users.AnyAsync(u => u.Email.ToLower() == email, ct);
        if (exists) throw new InvalidOperationException("El correo ya está registrado.");

        // 3) Hash de password
        var passwordHash = hasher.Hash(req.Password);

       
        var user = new User
{
    Email = email,
    PasswordHash = passwordHash, 
    Role = string.IsNullOrWhiteSpace(req.Role) ? "User" : req.Role,
    CreatedAt = DateTime.UtcNow
};

        db.Users.Add(user);
        await db.SaveChangesAsync(ct);

        // 5) JWT
        var token = jwt.CreateToken(user);

        // 6) Notificación
        await notifications.SendAsync($"Nuevo usuario: {user.Email} ({user.Role})", ct);

        return token;
    }
}

