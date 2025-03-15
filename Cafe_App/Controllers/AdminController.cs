using Cafe_App.Attributes.AuthAttributes;
using Cafe_App.Models.Admin;
using Cafe_App.Services;
using Cafe.Data.Interface.Models;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Enums.Users;
using Microsoft.AspNetCore.Mvc;

namespace Cafe_App.Controllers
{
    [IsAdmin]
    public class AdminController : Controller
    {
        private IUserRepository<UserData> _userRepository;
        private EnumHelper _enumHelper;

        public AdminController(
            IUserRepository<UserData> userRepositry, 
            EnumHelper enumHelper)
        {
            _userRepository = userRepositry;
            _enumHelper = enumHelper;
        }

        public IActionResult Users()
        {
            var users = _userRepository
                .GetAll()
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Name = x.Login,
                    Roles = _enumHelper.GetNames(x.Role)
                })
                .ToList();

            var viewModel = new AdminUserViewModel();
            viewModel.Users = users;

            viewModel.Roles = _enumHelper.GetSelectListItems<Roles>();

            return View(viewModel);
        }

        public IActionResult DeleteUser(int id)
        {
            _userRepository.Delete(id);
            return RedirectToAction("Users");
        }

        public IActionResult UpdateRole(Roles role, int userId)
        {
            _userRepository.UpdateRole(userId, role);
            return RedirectToAction("Users");
        }
    }
}
