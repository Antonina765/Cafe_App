namespace Cafe_App.Models.Cafe;

public class CafeInfoViewModel
{
    public int CafeId { get; set; }
    
    public string Title { get; set; }
    
    public string Address { get; set; }
    
    public string Description { get; set; }
    public IFormFile? MenuImage { get; set; }
    
    public bool AvailableTable { get; set; }
    
    public int CountOfTables { get; set; }
}