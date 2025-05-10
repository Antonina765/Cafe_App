using System.ComponentModel.DataAnnotations;

namespace Cafe_App.Models.Booking;

public class TableBookingViewModel
{
    [Required(ErrorMessage = "Выберите дату и время бронирования")]
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Укажите количество гостей")]
    public int GuestsCount { get; set; }

    [Required(ErrorMessage = "Введите номер телефона")]
    [Phone(ErrorMessage = "Некорректный номер телефона")]
    public string PhoneNumber { get; set; }
    
    public int CafeId { get; set; }
}