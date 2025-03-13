using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;

namespace Cafe.Data.Repositories;

public class CafeRepository : BaseRepository<CafeData>, ICafeRepository<CafeData>
{
    public CafeRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public int CreateCafe(CafeData cafeData, int currentUserId)
    {
        var creatorId = _webDbContext.Users.First(u => u.Id == currentUserId);

        cafeData.Creator = creatorId;
        return Add(cafeData);
    }

    public bool HasSimilarTitles(string cafeTitle)
    {
        return !_dbSet.Any(x => x.Title == cafeTitle);
    }
    
    public void UpdateImage(int id, string url)
    {
        var cafe = _dbSet.First(x => x.Id == id);

        cafe.ImageSrc = url;

        _webDbContext.SaveChanges();
    }

    public void UpdateName(int id, string newTitle)
    {
        var cafe = _dbSet.First(x => x.Id == id);

        cafe.Title = newTitle;

        _webDbContext.SaveChanges();
    }
}