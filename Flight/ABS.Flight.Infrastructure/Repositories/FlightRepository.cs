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

	public async Task<IEnumerable<FlightEntity>> GetAllFlightsAsync(int? pageNumber = 1, int? pageSize = 10)
	{ 
		const string sql = @"
			SELECT Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime
			FROM Flights
			ORDER BY Id
			OFFSET @offset ROWS
			FETCH NEXT @pageSize  ROWS ONLY";

		var offset = (pageNumber - 1) * pageSize;
		return await _dbConnection.QueryAsync<FlightEntity>(sql, new { Offset = offset, PageSize = pageSize });
	}

	public async Task<FlightEntity> GetFlightByFlightNumberAsync(string flightNumber)
	{
		const string sql = @"
			SELECT Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime
			FROM Flights
			WHERE FlightNumber = @FlightNumber";

		var flightEntity = await _dbConnection.QuerySingleOrDefaultAsync<FlightEntity>(sql, new { FlightNumber = flightNumber });

		if (flightEntity is null)
			return new FlightEntity();

		return flightEntity;
	}

	public async Task<FlightEntity> GetFlightByIdAsync(Guid id)
	{
		const string sql = @"
			SELECT Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime
			FROM Flights
			WHERE Id = @Id";

		var flightEntity = await _dbConnection.QueryFirstOrDefaultAsync<FlightEntity>(sql, new { Id = id });
		if (flightEntity is null)
			return new FlightEntity();	

		return flightEntity;
	}
}