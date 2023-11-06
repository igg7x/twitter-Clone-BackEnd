using AutoMapper;
using System.Net;
using System.Web.Http;
using Tw_Clone.Dto.Tweet;
using Tw_Clone.Dto.User;
using Tw_Clone.Models;
using Tw_Clone.Repositories;

namespace Tw_Clone.Services
{
    public class TweetService
    {


        private readonly ITweetRepository _tweetRepo;
        //private readonly UserService _userService ;
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

            tweet.Likes = GetLikesByTweetId(tw.Id);

            tweet.Reposts = GetRepostsByTweetId(tw.Id);

            return tweet; 
        }

        private List<UsersDto>? GetLikesByTweetId(int id)
        {
            var users = db.Tweetslikes.Where(tl => tl.TweetId == id && tl.User.DeletedAt == null).Select(tl => tl.User.Username);
            Console.WriteLine(users); 
            return _mapper.Map<List<UsersDto>>(users);
        }

        private    List<UsersDto>? GetRepostsByTweetId(int id)
        {
            var users = db.Tweetsreposts.Where(tl => tl.TweetId == id && tl.User.DeletedAt == null).Select(tl => tl.User.Username);
            return _mapper.Map<List<UsersDto>>(users);
        }

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
    }
}
