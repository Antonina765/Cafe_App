namespace Cafe.Data.Interface.Models;

public interface IBookingData : IBaseModel
{
    int CafeId { get; set; }
    int UserId { get; set; }
    string UserName { get; set; } 
    string PhoneNumber { get; set; }
    int GuestsCount { get; set; }
    DateTime BookingDateTime { get; set; }
}