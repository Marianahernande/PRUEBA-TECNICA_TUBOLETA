using ECommerce.Application.Notifications;

namespace ECommerce.Infrastructure.Notifications;

public class ConsoleNotificationService : INotificationService
{
    public Task SendAsync(string message, CancellationToken ct = default)
    {
        Console.WriteLine($"[NOTIFY] {DateTime.UtcNow:O} - {message}");
        return Task.CompletedTask;
    }
}
