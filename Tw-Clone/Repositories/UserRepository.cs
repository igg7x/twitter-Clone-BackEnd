using Tw_Clone.Models;

namespace Tw_Clone.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> Update(User entity);
    }
    public class UserRepository : Repository<User>, IUserRepository
    {


        private readonly TwcloneContext _db;

        public UserRepository(TwcloneContext db) : base(db)
        {
            _db = db;
        }

        public async Task<User> Update(User entity)
        {
           _db.Users.Add(entity);
            await Save();
            return entity;
        }
    }
}
