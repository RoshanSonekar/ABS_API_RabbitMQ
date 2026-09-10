using ABS.Booking.Core.Entities;
using ABS.Booking.Core.Repositories;
using BuildingBlocks.CQRS;

namespace ABS.Booking.Application.Query;
public record GetBookingResult(BookingEntity BookingEntity);
public record GetBookingQuery(Guid Id) : IQuery<GetBookingResult>;

public class BookingQueryHandler(IBookingRepository bookingRepository)
	: IQueryHandler<GetBookingQuery, GetBookingResult> 
{
	public async Task<GetBookingResult> Handle(GetBookingQuery request, CancellationToken cancellationToken)
	{
		var bookingEntity = await bookingRepository.GetBookingByIdAsync(request.Id);

		if(bookingEntity is null)
			return new GetBookingResult(new BookingEntity());

		return new GetBookingResult(bookingEntity);
	}
}	 
