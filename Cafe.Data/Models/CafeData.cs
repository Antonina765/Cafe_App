using Cafe.Data.Interface.Models;

namespace Cafe.Data.Models;

public class CafeData : BaseModel, ICafeData
{
    public string Title { get; set; }
    
    public string Adres { get; set; }
    
    public double Rank { get; set; }
    
    public string ImageSrc { get; set; }
    
    public UserData Creator { get; set; }
    
    public int CreatorId { get; set; }
}