using Cafe.Data.Interface.Models;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Enums.Users;

namespace Cafe.Data.Repositories;

public class UserRepository : BaseRepository<UserData>, IUserRepository<UserData>
{
    public UserRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public override int Add(UserData data)
    {
        throw new NotImplementedException("User method Register to create a new User");
    }
    
    public bool CheckIsNameAvailable(string userName)
    {
        return !_dbSet.Any(x => x.Login == userName);
    }
    
    public string GetAvatarUrl(int userId)
    {
        return _dbSet.First(x => x.Id == userId).AvatarUrl;
    }
    
    public string GetUserName(int userId)
    {
        return _dbSet.First(x => x.Id == userId).Login;
    }

    public bool IsAdminExist()
    {
        return _dbSet.Any(x => x.Role.HasFlag(Roles.Admin));
    }
    
    public IUser? Login(string login, string password)
    {
        var brokenPassword = BrokePassword(password);

        return _dbSet.FirstOrDefault(x => x.Login == login && x.Password == brokenPassword);
    }

    public void Register(string login, string password, Roles role = Roles.User)
    {
        if (_dbSet.Any(x => x.Login == login))
        {
            throw new Exception();
        }
        
        var user = new UserData
        {
            Login = login,
            Password = BrokePassword(password),
            AvatarUrl = "/images/avatars/avatar.png",
            Role = role,
            Language = Languages.Ru,
        };

        _dbSet.Add(user);
        _webDbContext.SaveChanges();
    }
    
    public void UpdateAvatarUrl(int userId, string avatarUrl)
    {
        var user = _dbSet.First(x => x.Id == userId);
        user.AvatarUrl = avatarUrl;
        _webDbContext.SaveChanges();
    }

    public void UpdateRole(int userId, Roles role)
    {
        var user = _dbSet.First(x => x.Id == userId);
        user.Role = role;
        _webDbContext.SaveChanges();
    }
    
    public void UpdateLocal(int userId, Languages language)
    {
        var user = _dbSet.First(x => x.Id == userId);

        user.Language = language;

        _webDbContext.SaveChanges();
    }

    private string BrokePassword(string originalPassword)
    {
        // jaaaack
        // jacke
        // jack
        var brokenPassword = originalPassword.Replace("a", "");

        // jck
        return brokenPassword;
    }
}