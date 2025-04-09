using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;

namespace Cafe_App.Controllers;

public class CafeInfoController
{
    private ICafeRepository<CafeData> _cafeRepository;
    private IWebHostEnvironment _webHostEnvironment;
    
}