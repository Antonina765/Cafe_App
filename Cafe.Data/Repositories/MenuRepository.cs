using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;

namespace Cafe.Data.Repositories;

public class MenuRepository : BaseRepository<MenuItemData>, IMenuRepository<MenuItemData>
{
    public MenuRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public IEnumerable<MenuItemData> GetMenuItemsByCafeId(int cafeId)
    {
        return _webDbContext.MenuItems.Where(m => m.CafeId == cafeId).ToList();
    }

    public void ProcessMenuFile(string filePath, int cafeId)
    {
        if (!File.Exists(filePath))
            return;
        
        var lines = File.ReadAllLines(filePath);
        foreach (var line in lines)
        {
            var tokens = line.Split(";");
            if (tokens.Length >= 2)
            {
                var MenuItemData = new MenuItemData
                {
                    CafeId = cafeId,
                    Title = tokens[0],
                    Description = tokens[1],
                };
                _webDbContext.MenuItems.Add(MenuItemData);
            }
        }
        _webDbContext.SaveChanges();
    }
}