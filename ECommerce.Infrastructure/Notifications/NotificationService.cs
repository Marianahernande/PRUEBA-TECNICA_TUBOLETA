using ECommerce.Application.Notifications;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace ECommerce.Infrastructure.Notifications;

public sealed class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;
    public NotificationService(ILogger<NotificationService> logger) => _logger = logger;

    // firma 1 (mensaje simple)
    public Task SendAsync(string message, CancellationToken ct = default)
    {
        _logger.LogInformation("NOTIFY: {Message}", message);
        return Task.CompletedTask;
    }

    // firma 2 (dirigida a usuario)
    public Task SendAsync(int userId, string title, string body, CancellationToken ct = default)
    {
        _logger.LogInformation("NOTIFY to {UserId}: {Title} - {Body}", userId, title, body);
        return Task.CompletedTask;
    }
}
