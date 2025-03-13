using System.Linq.Expressions;
using Cafe.Data.Interface.Repositories;
using Cafe.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cafe.Data.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T>
        where T : BaseModel
    {
        protected WebDbContext _webDbContext;
        protected DbSet<T> _dbSet;

        public BaseRepository(WebDbContext webDbContext)
        {
            _webDbContext = webDbContext;
            _dbSet = webDbContext.Set<T>();
        }

        public virtual int Add(T data)
        {
            _webDbContext.Add(data);
            _webDbContext.SaveChanges();

            return data.Id;
        }

        public virtual bool Any()
        {
            return _dbSet.Any();
        }

        public int Count()
        {
            return _dbSet.Count();
        }

        public virtual void Delete(T data)
        {
            _dbSet.Remove(data);
            _webDbContext.SaveChanges();
        }

        public virtual void Delete(int id)
        {
            var data = Get(id);
            Delete(data);
        }

        public virtual T? Get(int id)
        {
            return _dbSet.FirstOrDefault(x => x.Id == id);
        }

        public virtual IEnumerable<T> GetAll()
        {
            return _dbSet.ToList();
        }

        protected virtual IQueryable<T> SortAndGetAll(string fieldForSort)
        {
            return SortAndGetAll(_dbSet, fieldForSort);
        }

        protected virtual IQueryable<T> SortAndGetAll(IQueryable<T> source, string fieldForSort)
        {
            // for example: model
            var paramExp = Expression.Parameter(typeof(T), "model");

            // For exmaple fieldForSort == "Manga.User.Id";

            var propertiesNames = fieldForSort.Split(".");
            // for example: model.Manga
            var propertyExp = Expression.Property(paramExp, propertiesNames[0]);
            for (int i = 1; i < propertiesNames.Length; i++)
            {
                propertyExp = Expression.Property(propertyExp, propertiesNames[i]);
            }

            // for example: model.Name
            var propertyExpAsObject = Expression.Convert(propertyExp, typeof(object));

            // for example: model => model.Name
            var linqExp = Expression.Lambda<Func<T, object>>(
                propertyExpAsObject,
                paramExp
                );

            return source.OrderBy(linqExp);
        }
    }