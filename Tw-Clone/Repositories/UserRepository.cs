using Tw_Clone.Models;

namespace Tw_Clone.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Update(User entity);
        Task Delete(User entity);
    }
    public class UserRepository : Repository<User>, IUserRepository
    {


        private readonly TwcloneContext _db;

        public UserRepository(TwcloneContext db) : base(db)
        {
            _db = db;
        }

        public async Task  Delete(User entity)
        {
            entity.DeletedAt = DateTime.Now; 
            _db.Update(entity);
            await Save();
        }

        public async Task<User> Update(User entity)
        {
           _db.Users.Add(entity);
            await Save();
            return entity;
        }
    }
}
