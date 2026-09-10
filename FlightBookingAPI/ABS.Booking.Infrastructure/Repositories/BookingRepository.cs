using ABS.Booking.Core.Entities;
using ABS.Booking.Core.Repositories;
using Dapper;
using System.Data;

namespace ABS.Booking.Infrastructure.Repositories;

public class BookingRepository : IBookingRepository
{
	private readonly IDbConnection _dbConnection;
	public BookingRepository(IDbConnection dbConnection)
	{
		_dbConnection = dbConnection;
	}

	public async Task AddBookingAsync(BookingEntity booking)
	{
		const string sql = @"
			INSERT INTO Bookings (Id, FlightId, PassengerName, SeatNumber, BookingDate)
			VALUES (@Id, @FlightId, @PassengerName, @SeatNumber, @BookingDate)";	
		
		await _dbConnection.ExecuteAsync(sql, booking);
	}

	public async Task<BookingEntity> GetBookingByIdAsync(Guid id)
	{
		const string sql = @"
			SELECT Id, FlightId, PassengerName, SeatNumber, BookingDate
			FROM Bookings
			WHERE Id = @Id";

		var bookingEntity =  await _dbConnection.QuerySingleOrDefaultAsync<BookingEntity>(sql, new { Id = id });
		if(bookingEntity == null) 
			return new BookingEntity();

		return bookingEntity;
	}
}
