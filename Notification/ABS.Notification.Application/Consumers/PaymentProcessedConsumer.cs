using ABS.Notification.Application.Command;
using BuildingBlocks.Transit.Contracts.EventBus.Messages;
using MassTransit;
using MediatR;

namespace ABS.Notification.Application.Consumers;

public class PaymentProcessedConsumer :IConsumer<PaymentProcessedEvent>
{
	private readonly IMediator _mediator;
	public PaymentProcessedConsumer(IMediator mediator)
	{
		_mediator = mediator;
	}

	public async Task Consume(ConsumeContext<PaymentProcessedEvent> context)
	{
		var message = context.Message;
		var notificationMsg = $"Payment of {message.Amount} for Booking Id {message.BookingId} has been processed successfully on {message.PaymentDate}. Payment Id: {message.PaymentId}";

		Console.WriteLine($"Received payment processed event: Booking Id = {message.BookingId}, PaymentId = {message.PaymentId}, " +
											$"Amount = {message.Amount}, PaymentDate = {message.PaymentDate}");
		
		
		var notificationCommand = new SendNotificationCommand("roshansonekar@gmail.com", notificationMsg, "Email", DateTime.UtcNow); 
		await _mediator.Send(notificationCommand);

		//await Task.CompletedTask;
	}
}
