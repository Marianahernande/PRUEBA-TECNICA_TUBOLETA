namespace ECommerce.Application.Notifications;

public interface INotificationService
{
    Task SendAsync(string message, CancellationToken ct = default);
}