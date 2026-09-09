namespace ABS.Booking.Core.Entities;

public class BookingEntity
{
	public Guid Id { get; set; }
	public Guid FlightId { get; set; } = Guid.Empty;
	public string PassengerName { get; set; } =default!;
	public string SeatNumber { get; set; } = default!;
	public DateTime BookingDate { get; set; } = DateTime.UtcNow;
}
