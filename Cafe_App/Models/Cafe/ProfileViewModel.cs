namespace Cafe_App.Models.Cafe;

public class ProfileViewModel
{
    public string UserName { get; set; }
    public string AvatarUrl { get; set; }

    public List<UserBookingViewModel> Bookings { get; set; } = new List<UserBookingViewModel>();
}

public class UserBookingViewModel
{
    public DateTime BookingDate { get; set; }
    public string CafeTitle { get; set; }
}