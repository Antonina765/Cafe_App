using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;

namespace Cafe.Data.Repositories;

public class BookingRepository: BaseRepository<BookingData>, IBookingRepository<BookingData>
{
    public BookingRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public void CreateBooking(BookingData booking)
    {
        booking.BookingDateTime = booking.BookingDateTime.ToUniversalTime();
        _webDbContext.Bookings.Add(booking);
        _webDbContext.SaveChanges();
    }

    public IEnumerable<BookingData> GetAllBookingsByCafeId(int cafeId)
    {
        return _webDbContext.Bookings.Where(b => b.CafeId == cafeId).ToList();
    }

    public IEnumerable<BookingData> GetBookingsByUserId(int userId)
    {
        return _webDbContext.Bookings.Where(b => b.UserId == userId).ToList();
    }
    
    public IEnumerable<BookingData> GetAllBookings()
    {
        return _webDbContext.Bookings.ToList();
    }
}