namespace Cafe.Data.Interface.Models;

public interface ICafeData : IBaseModel
{
    string Title { get; set; }
    
    string Address { get; set; }
    
    double Rang { get; set; }
    
    string ImageSrc { get; set; }
}