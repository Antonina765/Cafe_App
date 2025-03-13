using Enums.Users;

namespace Cafe.Data.Interface.Repositories;

public interface IUserRepository<UserData>
{
    string GetAvatarUrl(int userId);
    string GetUserName(int userId);
    bool IsAdminExist();
    UserData? Login(string login, string password);
    
    //void Register(string login, string password, string avatarUrl, Role role = Role.User);
    
    void Register(string login, string password, Roles role = Roles.User);
    void UpdateAvatarUrl(int userId, string avatarUrl);
    void UpdateLocal(int userId, Languages language);
    void UpdateRole(int userId, Roles role);
    bool CheckIsNameAvailable(string userName);
}