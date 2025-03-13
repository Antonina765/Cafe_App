using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Enums.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Cafe.Data;

public class Seed
{
    public void Fill(IServiceProvider service)
    {
        using var di = service.CreateScope();

        UserFill(di);
    }

    private void UserFill(IServiceScope di)
    {
        var userRepositry = di.ServiceProvider.GetRequiredService<IUserRepository<UserData>>();
        if (userRepositry.IsAdminExist())
        {
            return;
        }

        userRepositry.Register("admin", "admin", Roles.Admin);
    }
}

