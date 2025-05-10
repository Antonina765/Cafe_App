using Cafe.Data.Interface.Models;

namespace Cafe.Data.Models;

public class MenuItemData : BaseModel, IMenuItemData
{
    public int CafeId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}