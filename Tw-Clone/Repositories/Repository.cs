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
        Task Delete(T entity);
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

        public Task Add(T entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(T entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetOne(Expression<Func<T, bool>>? filter = null)
        {
            throw new NotImplementedException();
        }

        public Task Save()
        {
            throw new NotImplementedException();
        }
    }
}
