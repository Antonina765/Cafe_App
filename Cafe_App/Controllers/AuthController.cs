using System.Security.Claims;
using Cafe_App.Models.Auth;
using Cafe_App.Services;
using Cafe.Data.Interface.Models;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;

namespace Cafe_App.Controllers;
public class AuthController : Controller
{ 
    public IUserRepository<UserData> _userRepository;
    private IWebHostEnvironment _webHostEnvironment;
    
    public AuthController(IUserRepository<UserData> userRepository, 
        IWebHostEnvironment webHostEnvironment)
    {
        _userRepository = userRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginUserViewModel viewModel)
    {
        var user = _userRepository.Login(viewModel.UserName, viewModel.Password);
            
        if (user is null)
        {
            ModelState.AddModelError(
                nameof(viewModel.UserName), 
                "Не правильный логин или пароль");
        }

        if (!ModelState.IsValid)
        {
            return View(viewModel);
        }

        //Good user

        var claims = new List<Claim>()
        {
            new Claim(AuthService.CLAIM_TYPE_ID, user.Id.ToString()),
            new Claim(AuthService.CLAIM_TYPE_NAME, user.Login),
            new Claim(AuthService.CLAIM_TYPE_ROLE, ((int)user.Role).ToString()),
            new Claim (ClaimTypes.AuthenticationMethod, AuthService.AUTH_TYPE_KEY),
        };

        var identity = new ClaimsIdentity(claims, AuthService.AUTH_TYPE_KEY);

        var principal = new ClaimsPrincipal(identity);

        HttpContext
            .SignInAsync(principal)
            .Wait();

        return RedirectToAction("Profile", "Cafe");
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Register(RegisterUserViewModel viewModel)
    {
        if (!_userRepository.CheckIsNameAvailable(viewModel.UserName))
        {
            return View(viewModel);
        }
        
        _userRepository.Register(
            viewModel.UserName,
            viewModel.Password);

        //_chatHub.Clients.All.NewMessageAdded($"Новый пользователь зарегестировался у нас на сайте. Его зовут {viewModel.UserName}");

        return RedirectToAction("Login");
    }
    
    [HttpPost] 
    public IActionResult RegisterFromProfile(RegisterUserViewModel viewModel) 
    { 
        if (ModelState.IsValid) 
        { 
            ModelState.AddModelError("", "Registration failed. Please try again."); 
        } 
        // Assuming you redirect back to the profile page if there's an error.
        return RedirectToAction("Profile", "Cafe"); 
    }

    public IActionResult Logout()
    {
        HttpContext
            .SignOutAsync()
            .Wait();

        return RedirectToAction("Profile", "Cafe");
    }
}