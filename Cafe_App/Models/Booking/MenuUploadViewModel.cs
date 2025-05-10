using System.ComponentModel.DataAnnotations;

namespace Cafe_App.Models.Booking;

public class MenuUploadViewModel
{
    [Required(ErrorMessage = "Выберите файл с меню")]
    public IFormFile MenuFile { get; set; }
}