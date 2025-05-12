using Enums.Users;
using Cafe.Data.Models;
using Cafe.Data.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Cafe_App.Models.Auth;
using Cafe_App.Services;
using Cafe_App.Hubs;
using Cafe.Data.Interface.Repositories;


namespace Cafe_App.Controllers
{
    public class AuthController : Controller
    {
        private IUserRepository<UserData> _userRepository;
        private IHubContext<ChatHub, IChatHub> _chatHub;

        public AuthController(IUserRepository<UserData> userRepository, 
            IHubContext<ChatHub, IChatHub> chatHub)
        {
            _userRepository = userRepository;
            _chatHub = chatHub;
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

            _chatHub.Clients.All.NewMessageAdded($"Новый пользователь зарегестировался у нас на сайте. Его зовут {viewModel.UserName}");

            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext
                .SignOutAsync()
                .Wait();

            return RedirectToAction("Profile", "Cafe");
        }
    }
}
