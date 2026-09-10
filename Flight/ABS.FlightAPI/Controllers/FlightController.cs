//using ABS.Flight.Application.Command;
using ABS.Flight.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABS.FlightAPI.Controllers
{
	[ApiController]
	[Route("api/flight")]
	public class FlightQueryController : ControllerBase
	{
		//private readonly ILogger<BookingController> _logger;
		private readonly IMediator _mediatR;

		public FlightQueryController(IMediator mediatR)
		{
			_mediatR = mediatR;
		}

		// Implementation for retrieving all using pagination	
		[HttpGet]
		public async Task<IActionResult> GetAllFlights([FromQuery] int? pageNumber = 1, [FromQuery] int? pageSize = 10)
		{
			var flights = await _mediatR.Send(new GetFlightsQuery(pageNumber, pageSize));
			return Ok(flights);
		}

		 
		// Implementation for retrieving a flight by ID		
		[HttpGet("{id}")]
		public async Task<IActionResult> GetFlightById(Guid id)
		{
			var flight = await _mediatR.Send(new GetFlightQuery(id));

			if (flight.FlightEntity.Id == Guid.Empty)
				return NotFound(new
				{
					Error = "FlightNotFound",
					Message = $"Flight with id {id} was not found."
				});

			return Ok(flight);
		}

		// Implementation for retrieving a flight by flight number		
		[HttpGet("flightNumber/{flightNumber}")]
		public async Task<IActionResult> GetFlightByFlightNumber(string flightNumber)
		{
			var flight = await _mediatR.Send(new GetFlightByFlightNumberQuery(flightNumber));

			if (flight.FlightEntity.Id == Guid.Empty)
				return NotFound(new
				{
					Error = "FlightNotFound",
					Message = $"Flight with flight number {flightNumber} was not found."
				});

			return Ok(flight);
		}
	}
}
