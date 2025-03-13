namespace Cafe.Data.Interface.Models;

public interface ICafeData
{
    string Title { get; set; }
    
    string Adres { get; set; }
    
    double Rank { get; set; }
    
    string ImageSrc { get; set; }
}