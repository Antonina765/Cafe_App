using Microsoft.AspNetCore.Mvc.Rendering;

namespace Cafe_App.Models.Admin;

public class AdminUserViewModel
{
    public List<UserViewModel> Users { get; set; }
        
    public List<SelectListItem> Roles { get; set; }
}