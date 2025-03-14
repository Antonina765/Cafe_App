using Cafe.Data.Interface.Models;

namespace Cafe.Data.Models;

public class CafeData : BaseModel, ICafeData
{
    public string Title { get; set; }
    
    public string Address { get; set; }
    
    public double Rang { get; set; }
    
    public string ImageSrc { get; set; }
    
}