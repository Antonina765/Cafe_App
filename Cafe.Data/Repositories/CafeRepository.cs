using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Enums.Users;

namespace Cafe.Data.Repositories;

public class CafeRepository : BaseRepository<CafeData>, ICafeRepository<CafeData>
{
    public CafeRepository(WebDbContext webDbContext) : base(webDbContext)
    {
    }

    public int CreateCafe(CafeData cafeData, int currentUserId)
    {
        var user = _webDbContext.Users.First(x => x.Id == currentUserId);

        // Проверяем, что пользователь является администратором
        if (user.Role != Roles.Admin)
        {
            throw new UnauthorizedAccessException("Только администраторы могут добавлять данные.");
        }
        
        var a = Add(cafeData);
        return a;
    }

    public bool HasSimilarTitles(string cafeTitle)
    {
        return _dbSet.Any(x => x.Title == cafeTitle);
    }
    
    public void UpdateImage(int id, string url)
    {
        var cafe = _dbSet.First(x => x.Id == id);

        cafe.ImageSrc = url;

        _webDbContext.SaveChanges();
    }

    public bool IsTitleUniq(string title)
    {
        return !_dbSet.Any(x => x.Title == title);
    }
    
    public void UpdateTitle(int id, string newTitle)
    {
        var cafe = _dbSet.First(x => x.Id == id);

        cafe.Title = newTitle;

        _webDbContext.SaveChanges();
    }
}