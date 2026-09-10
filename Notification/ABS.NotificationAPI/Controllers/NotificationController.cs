using ABS.Notification.Application.Command; 
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ABS.NotificationAPI.Controllers
{
	[ApiController]
	[Route("api/notification")]
	public class NotificationController : ControllerBase
	{
		//private readonly ILogger<BookingController> _logger;
		private readonly IMediator _mediatR;

		public NotificationController(IMediator mediatR)
		{
			_mediatR = mediatR;
		}

		[HttpPost]
		public async Task<IActionResult> LogNotification([FromBody] AddNotificationCommand command)
		{
			var result = await _mediatR.Send(command);
			return CreatedAtAction(nameof(GetLoggedNotification), new { id = result.Id }, result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetLoggedNotification(Guid id)
		{

			return Ok((new
			{
				 Id= id,
				Message = $"Notification logged with id	{id}"
			}));
		}
	}
}
