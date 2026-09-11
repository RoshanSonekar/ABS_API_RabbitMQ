using ABS.Notification.Core.Entities;
using ABS.Notification.Core.Repositories;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace ABS.Notification.Application.Command;

public record SendNotificationResult(Guid Id, bool IsSuccess);
public record SendNotificationCommand(string Recipient, string Message, string Type, DateTime SentAt) : ICommand<SendNotificationResult>;

public class NotificationCommandHandler(INotificationRepository notificationRepository)
	: ICommandHandler<SendNotificationCommand, SendNotificationResult>
{
	public async Task<SendNotificationResult> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
	{
		Console.WriteLine($"Handling AddNotificationCommand: Recipient={request.Recipient}, Message={request.Message}, Type={request.Type}");

		var notification = new NotificationEntity
		{
			Id = Guid.NewGuid(),
			Recipient = request.Recipient,
			Message = request.Message,
			Type = request.Type,
			SentAt = request.SentAt
		};

		await notificationRepository.LogNotificationAsync(notification);
		
		Console.WriteLine($"Notification logged successfully. Id = {notification.Id}");
		return new SendNotificationResult(notification.Id, true);
	}
}