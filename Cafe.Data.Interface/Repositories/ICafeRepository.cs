using Cafe.Data.Interface.Models;

namespace Cafe.Data.Interface.Repositories;

public interface ICafeRepository<CafeData>
{
    int CreateCafe(CafeData cafeData, int currentUserId);
    
    bool HasSimilarTitles(string cafeTitle);
}