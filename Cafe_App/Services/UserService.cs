using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Cafe.Data.Repositories;

namespace Cafe_App.Services;

public class UserService
{
    private AuthService _authService;
    private IUserRepository<UserData> _userRepository;

    public const string DEFAULT_AVATAR = "/images/avatar-default.webp";

    public UserService(AuthService authService, IUserRepository<UserData> userRepository)
    {
        _authService = authService;
        _userRepository = userRepository;
    }

    public string GetAvatar()
    {
        var userId = _authService.GetUserId();
        if (userId is null)
        {
            return DEFAULT_AVATAR;
        }

        var user = _userRepository.Get(userId.Value);
        return user.AvatarUrl;
    }
}