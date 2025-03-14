using Cafe.Data.Interface.Models;
using Enums.Users;

namespace Cafe.Data.Interface.Models;

public interface IUser : IBaseModel
{
    public string Login { get; set; }
    public string Password { get; set; }
    public string AvatarUrl { get; set; }

    public Languages Language { get; set; }
    public Roles Role { get; set; }
}
