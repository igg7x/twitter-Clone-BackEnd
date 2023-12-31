﻿using Tw_Clone.Models;

namespace Tw_Clone.Repositories
{

    public interface ITweetRepository : IRepository<Tweet>
    {
        Task<Tweet> Update(Tweet entity);

        Task Delete(Tweet entity);
    }
    public class TweetRepository : Repository<Tweet>, ITweetRepository
    {

        private readonly TwcloneContext _db;

        public TweetRepository(TwcloneContext db) : base(db)
        {
            _db = db;
        }

        public  async  Task  Delete(Tweet entity)
        {
             entity.DeletedAt = DateTime.Now;
            await _db.SaveChangesAsync();
        }

        public async Task<Tweet> Update(Tweet entity)
        {
           _db.Tweets.Update(entity);
            await Save(); 
            return entity;
        }
    }
}
