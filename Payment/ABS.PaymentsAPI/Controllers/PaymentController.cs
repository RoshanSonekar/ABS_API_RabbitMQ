using ABS.Payments.Application.Command;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABS.PaymentsAPI.Controllers
{
	[ApiController]
	[Route("api/notification")]
	public class PaymentController : ControllerBase
	{
		//private readonly ILogger<BookingController> _logger;
		private readonly IMediator _mediatR;

		public PaymentController(IMediator mediatR)
		{
			_mediatR = mediatR;
		}

		[HttpPost]
		public async Task<IActionResult> LogNotification([FromBody] ProcessPaymentCommand command)
		{
			var result = await _mediatR.Send(command);
			return CreatedAtAction(nameof(ProcessPayement), new { id = result.Id }, result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> ProcessPayement(Guid id)
		{
			return Ok((new
			{
				Id = id,
				Message = $"Payment created with id	{id}"
			}));
		}
	}
}
