using Cafe.Data.Interface.Models;
using Enums.Users;

namespace Cafe.Data.Interface.Repositories;

public interface IUserRepository<T> : IBaseRepository<T>
    where T : IUser
{
    string GetAvatarUrl(int userId);
    string GetUserName(int userId);
    bool IsAdminExist();
    IUser? Login(string login, string password);
    
    //void Register(string login, string password, string avatarUrl, Role role = Role.User);
    
    void Register(string login, string password, Roles role = Roles.User);
    void UpdateAvatarUrl(int userId, string avatarUrl);
    void UpdateLocal(int userId, Languages language);
    void UpdateRole(int userId, Roles role);
    bool CheckIsNameAvailable(string userName);
}