using MassTransit;
using BuildingBlocks.Transit.Contracts.EventBus.Messages;
using MediatR;
using ABS.Payments.Application.Command;

namespace ABS.Payments.Application.Consumers
{
	public class FlightBookedConsumer : IConsumer<FlightBookedEvent>
	{
		private readonly IMediator _mediator;
		public FlightBookedConsumer(IMediator mediator)
		{
			_mediator = mediator;
		}

		public async Task Consume(ConsumeContext<FlightBookedEvent> context)
		{
			var flightBookedEvent = context.Message;
			var command = new ProcessPaymentCommand(99.99m, flightBookedEvent.BookingId, flightBookedEvent.BookingDate);

			//Console.WriteLine($"Received flight booked event: BookingId = {flightBookedEvent.BookingId}, " +
			//									$"FlightId = {flightBookedEvent.FlightId}, PassengerName = {flightBookedEvent.PassengerName}, " +
			//									$"SeatNumber = {flightBookedEvent.SeatNumber}, BookingDate = {flightBookedEvent	.BookingDate}");
			// Here you can implement the logic to process the flight booked event, e.g., initiate payment processing.
			await _mediator.Send(command);
		}
	}
}
