using Cafe.Data.Interface.Models;
using Enums.Users;

namespace Cafe.Data.Interface.Repositories;

public interface ICafeRepository<CafeData> : IBaseRepository<CafeData>
{
    int CreateCafe(CafeData cafeData, int currentUserId);
    
    bool HasSimilarTitles(string cafeTitle);
    
    void UpdateImage(int Id, string url);
    
    void UpdateTitle(int Id, string newTitle);
    
    bool IsTitleUniq(string title);
}