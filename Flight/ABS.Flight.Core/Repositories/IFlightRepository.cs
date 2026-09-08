using ABS.Flight.Core.Entities;

namespace ABS.Flight.Core.Repositories;

public interface IFlightRepository
{
	Task<IEnumerable<FlightEntity>> GetAllFlightsAsync();
	Task AddFlightAsync(FlightEntity flight);
	Task RemoveFlightAsync(FlightEntity flight); 
	//Task<FlightEntity> GetFlightByIdAsync(Guid id);
}
