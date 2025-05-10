using Cafe_App.Models.Cafe;
using Cafe_App.Services;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_App.Controllers;

public class CafeDetailsController : Controller
{
    private ICafeRepository<CafeData> _cafeRepository;
    private IMenuRepository<MenuItemData> _menuRepository;
    private IBookingRepository<BookingData> _bookingRepository;
    private AuthService _authService;
    private IWebHostEnvironment _webHostEnvironment;
    private AutoMapperCafe _cafeMapper;

    public CafeDetailsController(ICafeRepository<CafeData> cafeRepository,
        IMenuRepository<MenuItemData> menuRepository,
        IBookingRepository<BookingData> bookingRepository,
        AuthService authService,
        IWebHostEnvironment webHostEnvironment,
        AutoMapperCafe cafeMapper)
    {
        _cafeRepository = cafeRepository;
        _menuRepository = menuRepository;
        _bookingRepository = bookingRepository;
        _authService = authService;
        _webHostEnvironment = webHostEnvironment;
        _cafeMapper = cafeMapper;
    }

    [HttpGet]
    public IActionResult MoreInfo(int id)
    {
        var cafe = _cafeRepository.Get(id);
        if (cafe == null) return NotFound();
        
        bool isAdmin = _authService.IsAdmin();
        ViewBag.IsAdmin = isAdmin;
        
        var menuItems = _menuRepository.GetMenuItemsByCafeId(id);
        var viewModel = new CafeDetailsViewModel
        {
            CafeId = cafe.Id,
            Title = cafe.Title,
            Address = cafe.Address,
            MenuItems = menuItems,
            BookingList = isAdmin ? _bookingRepository.GetAllBookingsByCafeId(id) : null,
            BookingForm = !isAdmin ? new Cafe_App.Models.Booking.TableBookingViewModel { CafeId = cafe.Id } : null
        };
        return View("MoreInfo", viewModel);
    }
}