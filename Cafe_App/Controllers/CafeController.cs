using System.Security.Claims;
using Cafe_App.Attributes.AuthAttributes;
using Cafe_App.Models.Cafe;
using Cafe_App.Services;
using Cafe.Data.Interface.Models;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Enums.Users;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_App.Controllers;

public class CafeController : Controller
{
    private ICafeRepository<CafeData> _cafeRepository;
    private IUserRepository<UserData> _userRepository;
    private AuthService _authService;
    private IWebHostEnvironment _webHostEnvironment;
    private AutoMapperCafe _cafeMapper;

    public CafeController(ICafeRepository<CafeData> cafeRepository,
        IUserRepository<UserData> userRepository,
        AuthService authService,
        IWebHostEnvironment webHostEnvironment,
        AutoMapperCafe cafeMapper)
    {
        _cafeRepository = cafeRepository;
        _userRepository = userRepository;
        _authService = authService;
        _webHostEnvironment = webHostEnvironment;
        _cafeMapper = cafeMapper;
    }

    public IActionResult Index()
    {
        if (!_cafeRepository.Any())
        {
            GenerateDefaultCafe();
        }

        var cafesFromDb = _cafeRepository.GetAll();
        
        var cafesViewModels = cafesFromDb
            .Select(dbCafe =>
                new CafeIndexViewModel
                {
                    CafeId = dbCafe.Id,
                    Title = dbCafe.Title,
                    ImageSrc = dbCafe.ImageSrc,
                    Rang = dbCafe.Rang,
                    Address = dbCafe.Address,
                    //Tags = new List<string>(),
                    CanDelete = User.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "Admin")
                }
            )
            .ToList();
        
        
        
        return View(cafesViewModels);
        
    }

    private void GenerateDefaultCafe()
    {
        
    }

    [HttpGet]
    public IActionResult Create()
    {
        var viewModel = new CafeCreationViewModel();
        
        return View(viewModel);
    }

    [HttpPost]
    public IActionResult Create(CafeCreationViewModel viewModel)
    {
        if (!_authService.IsAdmin())
        {
            return Unauthorized("Только администраторы могут создавать новое кафе.");
        }
        
        if (_cafeRepository.HasSimilarTitles(viewModel.Title))
        {
            ModelState.AddModelError(
                nameof(CafeCreationViewModel.Title),
                "Слишком похожее название");
        }

        // Если ни файл не выбран, ни указан URL, добавляем ошибку модели
        if ((viewModel.ImageFile == null || viewModel.ImageFile.Length == 0) && string.IsNullOrWhiteSpace(viewModel.ImageSrc))
        {
            ModelState.AddModelError("Image",
                "Необходимо предоставить изображение: либо загрузить файл, либо указать URL.");
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        // Если файл был загружен – приоритет отдается ему
        string imagePath = null;
        if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var uploadsFolder = Path.Combine(webRootPath, "images", "cafes");

            // Создаем директорию, если ее нет
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Генерируем уникальное имя файла
            var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(viewModel.ImageFile.FileName)}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                viewModel.ImageFile.CopyTo(fileStream);
            }

            // Сохраняем относительный путь, который будет храниться в базе
            imagePath = $"/images/cafes/posts/{fileName}";
        }
        else
        {
            // Если файл не передан, используем URL из модели
            imagePath = viewModel.ImageSrc;
        }
        
        var currentUserId = _authService.GetUserId();
        
        var dataCafe = new CafeData
        {
            Title = viewModel.Title,
            ImageSrc = imagePath,
            Rang = viewModel.Rang,
            Address = viewModel.Address
        };

        _cafeRepository.CreateCafe(dataCafe, currentUserId!.Value);
        
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public IActionResult Profile()
    {
        var viewModel = new ProfileViewModel();
        var userId = _authService.GetUserId();
        
        if (userId != null)
        {
            viewModel.UserName = _userRepository.GetUserName(userId.Value);
            viewModel.AvatarUrl = _userRepository.GetAvatarUrl(userId!.Value);
        }
        else 
        {
            viewModel.UserName = "Guest";
            viewModel.AvatarUrl = "~/images/avatars/avatar.png";
        }
   
        return View(viewModel);
    }
    
    [IsAuthenticated]
    [HttpPost]
    public IActionResult UpdateAvatar(IFormFile avatar)
    {
        if(avatar == null || avatar.Length == 0)
        {
            return RedirectToAction("Profile");
        }
        var webRootPath = _webHostEnvironment.WebRootPath;

        var userId = _authService.GetUserId()!.Value;
        var avatarFileName = $"avatar-{userId}.jpg";

        var path = Path.Combine(webRootPath, "images", "avatars", avatarFileName);
        using (var fileStream = new FileStream(path, FileMode.Create))
        {
            avatar
                .CopyToAsync(fileStream)
                .Wait();
        }

        var avatarUrl = $"/images/avatars/{avatarFileName}";
        _userRepository.UpdateAvatarUrl(userId, avatarUrl);

        return RedirectToAction("Profile");
    }
    public IActionResult UpdateLocale(Languages language)
    {
        var userId = _authService.GetUserId()!.Value;
        _userRepository.UpdateLocal(userId, language);

        return RedirectToAction("Index");
    }
}