using ABS.Notification.Core.Entities;
namespace ABS.Notification.Core.Repositories;

public interface INotificationRepository
{
	Task LogNotificationAsync(NotificationEntity notification);
}

