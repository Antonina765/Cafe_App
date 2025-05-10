namespace Cafe.Data.Interface.Models;

public interface IMenuItemData
{ 
   int CafeId { get; set; }
   string Title { get; set; } 
   string Description { get; set; }
}