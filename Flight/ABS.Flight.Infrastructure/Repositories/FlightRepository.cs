using ABS.Flight.Core.Entities;
using ABS.Flight.Core.Repositories;
using Dapper;
using System.Data;

namespace ABS.Flight.Infrastructure.Repositories;

public class FlightRepository : IFlightRepository
{
	private readonly IDbConnection _dbConnection;
	public FlightRepository(IDbConnection dbConnection)
	{
		_dbConnection = dbConnection;
	}

	public async Task AddFlightAsync(FlightEntity flight)
	{
		const string sql = @"
			INSERT INTO Flights (Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime)
			VALUES (@Id, @FlightNumber, @Origin, @Destination, @DepartureTime, @ArrivalTime)";

		await _dbConnection.ExecuteAsync(sql, flight);
	}
	public async Task RemoveFlightAsync(Guid id)
	{
		const string sql = @"
			DELETE FROM Flights
			WHERE Id = @Id";

		await _dbConnection.ExecuteAsync(sql, new { Id = id });
	}

	public async Task<IEnumerable<FlightEntity>> GetAllFlightsAsync()
	{
		const string sql = @"
			SELECT Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime
			FROM Flights";

		return await _dbConnection.QueryAsync<FlightEntity>(sql);
	}

	public async Task<FlightEntity> GetFlightByIdAsync(Guid id)
	{
		const string sql = @"
			SELECT Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime
			FROM Flights
			WHERE Id = @Id";

		return await _dbConnection.QuerySingleAsync<FlightEntity>(sql, new { Id = id });
	}
}