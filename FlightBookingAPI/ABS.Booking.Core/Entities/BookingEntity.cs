namespace ABS.Booking.Core.Entities;

public class BookingEntity
{
	public Guid id { get; set; }
	public Guid FlightId { get; set; } = Guid.Empty;
	public string PassangerName { get; set; } = string.Empty;
	public string SeatNumber { get; set; } = string.Empty;
	public DateTime BookingDate { get; set; } = DateTime.UtcNow;
}
