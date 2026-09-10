using ABS.Notification.Core.Entities;
using ABS.Notification.Core.Repositories;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace ABS.Notification.Application.Command;

public record AddNotificationResult(Guid Id, bool IsSuccess);
public record AddNotificationCommand(string Recipient, string Message, string Type, DateTime SentAt) : ICommand<AddNotificationResult>;

public class NotificationCommandHandler(INotificationRepository notificationRepository)
	: ICommandHandler<AddNotificationCommand, AddNotificationResult>
{
	public async Task<AddNotificationResult> Handle(AddNotificationCommand request, CancellationToken cancellationToken)
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
		return new AddNotificationResult(notification.Id, true);
	}
}