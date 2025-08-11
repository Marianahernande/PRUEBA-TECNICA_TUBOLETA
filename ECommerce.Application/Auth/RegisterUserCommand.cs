using MediatR;

namespace ECommerce.Application.Auth;

public class RegisterUserCommand : IRequest<string>  
{
    public string Email { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string? Role { get; set; }
}