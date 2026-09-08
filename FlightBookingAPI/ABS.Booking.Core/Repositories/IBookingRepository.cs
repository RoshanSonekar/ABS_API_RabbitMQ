using ABS.Booking.Core.Entities;

namespace ABS.Booking.Core.Repositories;

public interface IBookingRepository
{
	Task<BookingEntity> GetBookingByIdAsync(Guid id);
	Task AddBookingAsync(BookingEntity booking);
}
