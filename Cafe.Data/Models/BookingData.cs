using Cafe.Data.Interface.Models;

namespace Cafe.Data.Models;

public class BookingData : BaseModel, IBookingData
{
    public int CafeId { get; set; }
    public int UserId { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public int GuestsCount { get; set; }
    public DateTime BookingDateTime { get; set; }
}