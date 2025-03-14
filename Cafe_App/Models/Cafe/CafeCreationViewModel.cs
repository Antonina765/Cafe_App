namespace Cafe_App.Models.Cafe;

public class CafeCreationViewModel
{ 
    public string Title { get; set; }
    
    public string ImageSrc { get; set; }
    
    public IFormFile ImageFile { get; set; }
    
    public double Rang { get; set; }
    
    public string Address { get; set; }
}