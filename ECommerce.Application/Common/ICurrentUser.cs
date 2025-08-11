namespace ECommerce.Application.Common;
public interface ICurrentUser
{
    int UserId { get; }
    string? Email { get; }
    string? Role  { get; }
}