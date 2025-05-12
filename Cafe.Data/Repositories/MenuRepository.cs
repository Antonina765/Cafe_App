using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;

namespace Cafe.Data.Repositories;

public class MenuRepository : BaseRepository<MenuItemData>, IMenuRepository<MenuItemData>
{
    public MenuRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public MenuItemData GetMenuItemsByCafeId(int cafeId)
    {
        return _webDbContext.MenuItems.FirstOrDefault(m => m.CafeId == cafeId);
    }

    public void ProcessMenuFile(string filePath, int cafeId)
    {
        if (!File.Exists(filePath))
            return;

        var menuItem = new MenuItemData
        {
            CafeId = cafeId,
            Title = filePath,
            Description = "aaa"
        };
        _webDbContext.MenuItems.Add(menuItem);
        _webDbContext.SaveChanges();
    }
}