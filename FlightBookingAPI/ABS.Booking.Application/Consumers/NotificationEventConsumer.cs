using BuildingBlocks.Transit.Contracts.EventBus.Messages;
using MassTransit;

namespace ABS.Booking.Application.Consumers;

public class NotificationEventConsumer : IConsumer<NotificationEvent>	
{	
	public async Task Consume(ConsumeContext<NotificationEvent> context)
	{
		var message = context.Message;
		Console.WriteLine($"Received notification event: Recipient = {message.Recipient}, " +
											$"Message =	{message.Message}, Type = {message.Type}");
		await Task.CompletedTask;
	}
}
