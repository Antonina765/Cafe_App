using Cafe.Data.Models;

namespace Cafe_App.Models.Booking;

public class MenuViewModel
{
    public IEnumerable<MenuItemData> Items { get; set; }
    public TableBookingViewModel Booking { get; set; }
}