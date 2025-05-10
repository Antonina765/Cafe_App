namespace Cafe.Data.Interface.Repositories;

public interface IMenuRepository<MenuItemData> : IBaseRepository<MenuItemData>
{
    IEnumerable<MenuItemData> GetMenuItemsByCafeId(int cafeId);
    void ProcessMenuFile (string filePath, int cafeId);
}