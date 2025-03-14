namespace Cafe_App.Models.Cafe;

public class CafeIndexViewModel
{
    public int CafeId { get; set; }
    
    public string Title { get; set; }
    public string ImageSrc { get; set; }
    
    public string Address { get; set; }
    public double Rang { get; set; }
    
    public bool CanDelete { get; set; }
}