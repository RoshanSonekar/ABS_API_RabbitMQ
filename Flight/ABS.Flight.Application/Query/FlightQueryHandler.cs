using ABS.Flight.Core.Entities;
using ABS.Flight.Core.Repositories;
using BuildingBlocks.CQRS;

namespace ABS.Flight.Application.Query;

// Query to get flight by ID
public record GetFlightResult(FlightEntity FlightEntity);
public record GetFlightQuery(Guid Id) : IQuery<GetFlightResult>;

public class FlightQueryHandler(IFlightRepository flightRepository)
	:IQueryHandler<GetFlightQuery, GetFlightResult>
{
	public async Task<GetFlightResult> Handle(GetFlightQuery query, CancellationToken cancellationToken)
	{
		var flight = await flightRepository.GetFlightByIdAsync(query.Id); // Add cancellationToken if your repository method supports it
		return new GetFlightResult(flight);
	}
}


// Query to get flight by flight number
public record GetFlightByFlightNumberQuery(string flightNumber) : IQuery<GetFlightResult>;

public class GetFlightByFlightNumberQueryHandler(IFlightRepository flightRepository)
	: IQueryHandler<GetFlightByFlightNumberQuery, GetFlightResult>
{
	public async Task<GetFlightResult> Handle(GetFlightByFlightNumberQuery query, CancellationToken cancellationToken)
	{
		var flight = await flightRepository.GetFlightByFlightNumberAsync(query.flightNumber); // Add cancellationToken if your repository method supports it
		return new GetFlightResult(flight);
	}
}

// Query to get all flights with pagination
public record GetFlightsResult(IEnumerable<FlightEntity> FlightEntities);
public record GetFlightsQuery(int? PageNumber = 1, int? PageSize = 10) : IQuery<GetFlightsResult>; // get paginations valus config or UI

public class GetFlightsQueryHandler(IFlightRepository flightRepository) 
	:IQueryHandler<GetFlightsQuery, GetFlightsResult>	
{
	// Add cancellationToken if your repository method supports it
	public async Task<GetFlightsResult> Handle(GetFlightsQuery query, CancellationToken cancellationToken)
	{
		var flights = await flightRepository.GetAllFlightsAsync(query.PageNumber ?? 1, query.PageSize ?? 10);
		return new GetFlightsResult(flights);
	}
} 


