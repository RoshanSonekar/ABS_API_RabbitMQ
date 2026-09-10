using ABS.Flight.Core.Entities;
using ABS.Flight.Core.Repositories;
using BuildingBlocks.CQRS;
using FluentValidation;

namespace ABS.Flight.Application.Command;

public record AddFlightResult(Guid Id, bool IsSuccess);
public record AddFlightCommand(string FlightNumber, string Origin, string Destination, DateTime DepartureTime, DateTime ArrivalTime) : ICommand<AddFlightResult>;

public class AddFlightCommandValidator : AbstractValidator<AddFlightCommand>
{
	public AddFlightCommandValidator()
	{
		RuleFor(x => x.FlightNumber).NotEmpty().WithMessage("Flight number is required.");
		RuleFor(x => x.FlightNumber).MaximumLength(16).WithMessage("Flight number length must not exceed 16 chars.");
		RuleFor(x => x.Origin).NotEmpty().WithMessage("Origin is required.");
		RuleFor(x => x.Origin).MaximumLength(250).WithMessage("Origin length must not exceed 250 chars.");
		RuleFor(x => x.Destination).NotEmpty().WithMessage("Destination is required.");
		RuleFor(x => x.Destination).MaximumLength(250).WithMessage("Destination length must not exceed 250 chars.");
		RuleFor(x => x.DepartureTime).LessThan(DateTime.UtcNow).WithMessage("Departure time must not be in the past.");
		RuleFor(x => x.ArrivalTime).GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be after departure time.");
	}
}

public class FlightCommandHandler(IFlightRepository flightRepository)
	: ICommandHandler<AddFlightCommand, AddFlightResult>		
{
	public async Task<AddFlightResult> Handle(AddFlightCommand command, CancellationToken cancellationToken)
	{
		var flight = new FlightEntity
		{
			Id = Guid.NewGuid(),
			FlightNumber = command.FlightNumber,
			Origin = command.Origin,
			Destination = command.Destination,
			DepartureTime = command.DepartureTime,
			ArrivalTime = command.ArrivalTime
		};
		await flightRepository.AddFlightAsync(flight);
		return new AddFlightResult(flight.Id, true);
	}	
}
