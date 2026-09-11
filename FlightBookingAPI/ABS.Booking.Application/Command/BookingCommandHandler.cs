using ABS.Booking.Core.Entities;
using ABS.Booking.Core.Repositories;
using BuildingBlocks.CQRS;
using BuildingBlocks.Transit.Contracts.EventBus.Messages;
using FluentValidation;
using MassTransit;

namespace ABS.Booking.Application.Command;

public record AddBookingResult(Guid Id, bool IsSuccess);
public record AddBookingCommand(
	Guid FlightId, string PassengerName, string SeatNumber, DateTime BookingDate) 
	: ICommand<AddBookingResult>;

public class AddBookingCommandValidator : AbstractValidator<AddBookingCommand>
{
	public AddBookingCommandValidator()
	{
		RuleFor(x => x.FlightId).NotEmpty().WithMessage("Flight is required.");
		RuleFor(x => x.PassengerName).NotEmpty().WithMessage("Passenger name is required.");
		RuleFor(x => x.PassengerName).MaximumLength(250).WithMessage("Passenger name length must not exceed 250 chars.");
		RuleFor(x => x.SeatNumber).NotEmpty().WithMessage("Seat number is required.");
		RuleFor(x => x.SeatNumber).MaximumLength(16).WithMessage("Seat number length must not exceed 16 chars.");
		RuleFor(x => x.BookingDate).LessThan(DateTime.UtcNow).WithMessage("BookingDate date mustnot be in the past.");
		RuleFor(x => x.BookingDate).NotEmpty().WithMessage("BookingDate date is required.");
	}
}

public class BookingCommandHandler(IBookingRepository bookingRepository, IPublishEndpoint publishEndpoint)
	: ICommandHandler<AddBookingCommand, AddBookingResult>
{
	public async Task<AddBookingResult> Handle(AddBookingCommand command, CancellationToken cancellationToken)
	{
		var booking = new BookingEntity
		{
			Id = Guid.NewGuid(),
			FlightId = command.FlightId,
			PassengerName = command.PassengerName,
			SeatNumber = command.SeatNumber,
			BookingDate = command.BookingDate
		};

		await bookingRepository.AddBookingAsync(booking);

		// publish flight booked event to message broker
		await publishEndpoint.Publish(new FlightBookedEvent(
			booking.Id,
			booking.FlightId,
			booking.PassengerName,
			booking.SeatNumber,
			booking.BookingDate
			), cancellationToken);
		
		return new AddBookingResult(booking.Id, true);
	}
}
