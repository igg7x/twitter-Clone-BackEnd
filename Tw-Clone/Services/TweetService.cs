using AutoMapper;
using System.Net;
using System.Web.Http;
using Tw_Clone.Dto.Tweet;
using Tw_Clone.Dto.TweetsLike;
using Tw_Clone.Dto.TweetsReposts;
using Tw_Clone.Dto.User;
using Tw_Clone.Models;
using Tw_Clone.Repositories;

namespace Tw_Clone.Services
{
    public class TweetService
    {


        private readonly ITweetRepository _tweetRepo;
        private readonly IMapper _mapper;
        private readonly TwcloneContext db; 
 

        public TweetService( ITweetRepository repository , IMapper mapper , TwcloneContext twcloneContext ) {
            _tweetRepo = repository;
            _mapper = mapper;
            db = twcloneContext;
        }

        public async Task<List<TweetsDto>> GetAll()
        {
            var lista = await _tweetRepo.GetAll(tw=> tw.DeletedAt == null && db.Users.Any(u=> u.Id == tw.UserId  &&  u.DeletedAt == null));
             // select t.*from tweets t inner join users u  on t.user_id = u.Id where u.deleted_at is null and t.deleted_at is null;
            return _mapper.Map<List<TweetsDto>>(lista);
        }



        public async Task<List<TweetsDto>> GetAllByUserName(int userId)
        {
            var lista = await _tweetRepo.GetAll(tw => tw.UserId == userId);
            return _mapper.Map<List<TweetsDto>>(lista);
        }


        public async Task<TweetDto> GetById(int id)
        {
            var tw = await _tweetRepo.GetOne(tw => tw.Id == id);

            if (tw == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var user =   db.Users.Where(u => u.Id == tw.UserId).FirstOrDefault<User>()! ;

            var tweet =   _mapper.Map<TweetDto>(tw);

            tweet.UserName = user.Username;

            tweet.Comments = await  GetCommentsByTweetId(tw.Id);

            return tweet; 
        }


        // saber quien le dio like lo de followers tampoco 
        //private List<UsersDto>? GetLikesByTweetId(int id)
        //{
        //    var users = db.Tweetslikes.Where(tl => tl.TweetId == id );

        //    return _mapper.Map<List<UsersDto>>(users);
        //}

        //private    List<UsersDto>? GetRepostsByTweetId(int id)
        //{
        //    var users = db.Tweetsreposts.Where(tl => tl.TweetId == id && tl.User.DeletedAt == null).Select(tl => tl.User.Username);
        //    return _mapper.Map<List<UsersDto>>(users);    
        //}

        private async Task<List<TweetsDto>> GetCommentsByTweetId(int id)
        {
            var comments = await _tweetRepo.GetAll(t => db.Comments.Any(c => c.TweetedId == id && c.TweetCommentId == t.Id));
            //  select t.* from comments c inner join tweets t on t.id = c.tweet_comment_id where c.tweeted_id = 4   
            return _mapper.Map<List<TweetsDto>>(comments);
        }

        public async Task<TweetDto> Create(CreateTweetDto createTweetDto ,int userId)
        {
            var post = _mapper.Map<Tweet>(createTweetDto);
            post.UserId = userId; 

            await _tweetRepo.Add(post);

            return _mapper.Map<TweetDto>(post);
        }

        public async Task<TweetDto> UpdateById(int id, UpdateTweetDto updateTweetDto)
        {
            var post = await _tweetRepo.GetOne(u => u.Id == id);

            if (post == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var updated = _mapper.Map(updateTweetDto, post);

            return _mapper.Map<TweetDto>(await _tweetRepo.Update(updated));
        }


        public   async Task<List<TweetsDto>> GetAllRepostsByUserName(int id)
        {
           var reposts  = await _tweetRepo.GetAll(t => db.Tweetslikes.Any(tl => tl.UserId == id && tl.TweetId == t.Id));
            return _mapper.Map<List<TweetsDto>>(reposts);
        }


        public  async Task<List<TweetsDto>> GetAllLikesByUserName(int  id)
        {
            var likes = await _tweetRepo.GetAll(t => db.Tweetsreposts.Any(tl => tl.UserId == id && tl.TweetId == t.Id));
            return _mapper.Map<List<TweetsDto>>(likes);
        }

        public async Task DeleteById(int id)
        {
            var tweet = await _tweetRepo.GetOne(u => u.Id == id);

            if (tweet == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

          await _tweetRepo.Delete(tweet);
        }

        public async Task AddLikeToTweet(CreateTweetLikeDto dto, int id)
        {
            var likeAdded = _mapper.Map<Tweetslike>(dto);
             likeAdded.UserId = id;
            db.Tweetslikes.Add(likeAdded);
            await db.SaveChangesAsync();
        }

        public async  Task DeleteLikeToTweet(CreateTweetLikeDto dto, int id)
        {
            var existingLike =  db.Tweetslikes.Where(like => like.UserId == id && like.TweetId == dto.TweetId).FirstOrDefault();

            if (existingLike != null)
            {
                db.Tweetslikes.Remove(existingLike);
                await db.SaveChangesAsync();
            }
        }

         public  async  Task AddRepostToTweet(CreateTweetRepostDto dto, int id)
        {
            var repostAdded = _mapper.Map<Tweetsrepost>(dto);
            repostAdded.UserId = id;
            db.Tweetsreposts.Add(repostAdded);
            await db.SaveChangesAsync();
        }

        public async  Task DeleteRepostToTweet(CreateTweetRepostDto dto, int id)
        {
            var existingRepost = db.Tweetsreposts.Where(tr => tr.UserId == id && tr.TweetId == dto.TweetId).FirstOrDefault();

            if (existingRepost != null)
            {
                db.Tweetsreposts.Remove(existingRepost);
                await db.SaveChangesAsync();
            }
        }
        public async  Task CreateComment(int tweetedId, TweetDto tweet , int tweetcommentid)
        {
            
          
            
            var commentCreated = _mapper.Map<Comment>(tweet);



            commentCreated.TweetedId = tweetedId;
            commentCreated.TweetCommentId = tweetcommentid;
            db.Comments.Add(commentCreated);
            await db.SaveChangesAsync();

        }

        public bool GetLikeIfExists(int tweetid, int userid) {
           var like = db.Tweetslikes.Where(tl => tl.TweetId == tweetid && tl.UserId == userid).FirstOrDefault(); 
           return like != null ; 
        }
        public bool GetRepostsIfExists(int tweetId, int userid)
        {
            var repost = db.Tweetsreposts.Where(tr=> tr.TweetId == tweetId && tr.UserId == userid).FirstOrDefault();
            return repost != null;
        }
    }
}
