using ABS.Booking.Application.AddBooking;
using ABS.Booking.Application.GetBooking;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABS.BookingAPI.Controllers
{
	[ApiController]
	[Route("api/booking")]
	public class BookingController : ControllerBase
	{
		//private readonly ILogger<BookingController> _logger;
		private readonly IMediator _mediatR;

		public BookingController(IMediator mediatR)
		{
			_mediatR = mediatR;
		}

		[HttpPost]
		public async Task<IActionResult> AddBooking([FromBody]AddBookingCommand command)
		{
			var result = await _mediatR.Send(command);
			return CreatedAtAction(nameof(GetBookingById), new { id = result.Id }, result	);
		}

		// Implementation for retrieving a booking by ID		
		[HttpGet("{id}")]
		public async Task<IActionResult> GetBookingById(Guid id)
		{
			var booking = await _mediatR.Send(new GetBookingQuery(id));

			if(booking.BookingEntity.Id == Guid.Empty)
				return NotFound(new
				{
					Error = "BookingNotFound",
					Message = $"Booking with id {id} was not found."
				});

			return Ok(booking);
		}
	}
}
