namespace ABS.Notification.Core.Entities;
public class NotificationEntity
{
	public Guid Id { get; set; }
	public string Recipient { get; set; }= string.Empty; // Email address, phone number, etc.
	public string Message { get; set; }	= string.Empty;
	public string Type { get; set; } = string.Empty; // Email, SMS, Push Notification, etc.
	public DateTime SentAt { get; set; } = DateTime.UtcNow;
}
