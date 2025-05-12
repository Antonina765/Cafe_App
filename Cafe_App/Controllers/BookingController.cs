using Cafe_App.Models.Booking;
using Cafe_App.Services;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_App.Controllers;

public class BookingController : Controller
{
    private IWebHostEnvironment _webHostEnvironment;
    private IBookingRepository<BookingData> _bookingRepository;
    private IMenuRepository<MenuItemData> _menuRepository;
    private AuthService _authService;
    private AutoMapperCafe _cafeMapper;

    public BookingController(IBookingRepository<BookingData> bookingRepository,
        IMenuRepository<MenuItemData> menuRepository,
        IWebHostEnvironment webHostEnvironment,
        AuthService authService,
        AutoMapperCafe cafeMapper)
    {
        _bookingRepository = bookingRepository;
        _menuRepository = menuRepository;
        _webHostEnvironment = webHostEnvironment;
        _cafeMapper = cafeMapper;
        _authService = authService;
    }

    [HttpGet]
    public IActionResult UploadMenu()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> UploadMenu(MenuUploadViewModel viewModel, int cafeId)
    {
        if (viewModel.MenuFile == null || viewModel.MenuFile.Length == 0)
        {
            ModelState.AddModelError("MenuFile", "Select a file to upload");
            return View(viewModel);
        }
        
        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "menus");
        if (!Directory.Exists(uploadFolder))
        {
            Directory.CreateDirectory(uploadFolder);
        }

        var fileName = $"{Guid.NewGuid()}{Path.GetFileName(viewModel.MenuFile.FileName)}";
        var filePath = Path.Combine(uploadFolder, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            await viewModel.MenuFile.CopyToAsync(stream);
        }

        _menuRepository.ProcessMenuFile(filePath, cafeId);
        
        ViewBag.Message = "Menu uploaded";
        return RedirectToAction("Index", "Cafe");
    }

    [HttpPost]
    public IActionResult BookingTable(TableBookingViewModel bookingViewModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest("Некорректные данные в форме.");
        }

        int? currentUserId = _authService.GetUserId();
        
        if(currentUserId == null) return RedirectToAction("Register", "Auth");
        
        var userName = User.Identity.Name ?? "User";
        var booking = new BookingData
        {
            CafeId = bookingViewModel.CafeId,
            UserId = currentUserId.Value,
            UserName = userName,
            PhoneNumber = bookingViewModel.PhoneNumber,
            GuestsCount = bookingViewModel.GuestsCount,
            BookingDateTime = bookingViewModel.Date
        };
        
        _bookingRepository.CreateBooking(booking);
        
        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
        {
            return Json(new { success = true, message = "Стол успешно забронирован!" });
        }
        
        TempData["SuccessMessage"] = "Стол успешно забронирован!";
        return RedirectToAction("MoreInfo", "Cafe", new { id = bookingViewModel.CafeId });
    }

    [HttpGet]
    public IActionResult BookingList()
    {
        var bookings = _bookingRepository.GetAllBookings();
        return View(bookings);
    }
}