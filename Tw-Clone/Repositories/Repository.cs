using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Tw_Clone.Models;

namespace Tw_Clone.Repositories
{


    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> GetOne(Expression<Func<T, bool>>? filter = null);
        Task Add(T entity);
        Task Save();

    }
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly TwcloneContext _db;
        internal DbSet<T> dbSet;


        public Repository(TwcloneContext db)
        {
            _db = db;
            dbSet = _db.Set<T>();
        }

        public async Task Add(T entity)
        {
            await dbSet.AddAsync(entity);
            await Save();
        }

   

        public async  Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async  Task<T> GetOne(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async  Task Save()
        {
            await _db.SaveChangesAsync();
        }
    }
}
