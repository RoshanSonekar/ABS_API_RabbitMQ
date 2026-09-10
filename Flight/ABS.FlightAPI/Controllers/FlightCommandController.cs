using ABS.Flight.Application.Command;
using ABS.Flight.Application.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABS.FlightAPI.Controllers
{
	[ApiController]
	[Route("api/flight")]
	public class FlightCommandController : ControllerBase
	{
		//private readonly ILogger<BookingController> _logger;
		private readonly IMediator _mediatR;

		public FlightCommandController(IMediator mediatR)
		{
			_mediatR = mediatR;
		}

		[HttpPost]
		public async Task<IActionResult> AddFlight([FromBody] AddFlightCommand command)
		{
			var result = await _mediatR.Send(command);
			return CreatedAtAction(nameof(GetFlightById), new { id = result.Id }, result);
		}

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
	}
}
