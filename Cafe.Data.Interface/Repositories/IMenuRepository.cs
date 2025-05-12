namespace Cafe.Data.Interface.Repositories;

public interface IMenuRepository<MenuItemData> : IBaseRepository<MenuItemData>
{
    MenuItemData GetMenuItemsByCafeId(int cafeId);
    void ProcessMenuFile (string filePath, int cafeId);
}