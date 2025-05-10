namespace Cafe.Data.Interface.Repositories;

public interface IBookingRepository<BookingData> : IBaseRepository<BookingData>
{
    void CreateBooking (BookingData booking);
    IEnumerable<BookingData> GetAllBookingsByCafeId(int cafeId);
    IEnumerable<BookingData> GetBookingsByUserId(int userId);
    IEnumerable<BookingData> GetAllBookings();
}