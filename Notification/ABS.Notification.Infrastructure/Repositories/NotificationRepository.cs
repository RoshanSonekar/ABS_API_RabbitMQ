using ABS.Notification.Core.Entities;
using ABS.Notification.Core.Repositories;
using Dapper;
using System.Data;
using MassTransit;
using BuildingBlocks.Transit.Contracts.EventBus.Messages;


namespace ABS.Notification.Infrastructure.Repositories;

public class NotificationRepository : INotificationRepository
{
	private readonly IDbConnection _dbConnection;
	private readonly IPublishEndpoint _publishEndpoint;

	public NotificationRepository(IDbConnection dbConnection, IPublishEndpoint publishEndpoint)
	{
		_dbConnection = dbConnection;
		_publishEndpoint = publishEndpoint;	
	}

	// further implement the LogNotificationAsync method to insert a notification into the database or to publish to queue for actually sending the notification via email, sms, push notification, etc.
	// for now just insert the notification into the database
	public async Task LogNotificationAsync(NotificationEntity notification)
	{
		// publish event to the message bus for further processing (e.g., sending the notification)
		var notificationEvent = new NotificationEvent(notification.Recipient, notification.Message, notification.Type);
		await _publishEndpoint.Publish(notificationEvent);

		const string sql = @"
			INSERT INTO Notifications (Id, Recipient, Message, Type)
			VALUES (@Id, @Recipient, @Message, @Type)";

		await _dbConnection.ExecuteAsync(sql, notification);
	}
}
