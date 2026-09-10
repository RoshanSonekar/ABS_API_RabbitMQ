namespace ABS.Flight.Core.Entities;
public class FlightEntity
{
	public Guid Id { get; set; }
	public string FlightNumber { get; set; } = string.Empty;
	public string Origin { get; set; } = string.Empty;
	public string Destination { get; set; } = string.Empty;
	public DateTime DepartureTime { get; set; } = DateTime.UtcNow;
	public DateTime ArrivalTime { get; set; } = DateTime.UtcNow;
}
