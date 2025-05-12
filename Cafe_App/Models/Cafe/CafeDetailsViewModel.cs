using Cafe_App.Models.Booking;
using Cafe.Data.Models;

namespace Cafe_App.Models.Cafe;

public class CafeDetailsViewModel
{
    public int CafeId { get; set; }
    public string Title { get; set; }
    public string Address { get; set; }
    
    public string MenuItems { get; set; }
    public IEnumerable<BookingData> BookingList { get; set; }
    
    public TableBookingViewModel BookingForm { get; set; }
}