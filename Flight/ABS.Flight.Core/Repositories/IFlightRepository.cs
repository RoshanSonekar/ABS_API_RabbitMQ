using ABS.Flight.Core.Entities;

namespace ABS.Flight.Core.Repositories;

public interface IFlightRepository
{
	Task<IEnumerable<FlightEntity>> GetAllFlightsAsync(int? PageNumber = 1, int? PageSize = 10);
	Task AddFlightAsync(FlightEntity flight);
	Task RemoveFlightAsync(Guid id); 
	Task<FlightEntity> GetFlightByIdAsync(Guid id);
	Task<FlightEntity> GetFlightByFlightNumberAsync(string flightNumber);
}
