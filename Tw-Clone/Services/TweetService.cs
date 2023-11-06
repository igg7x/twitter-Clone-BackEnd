using AutoMapper;
using System.Net;
using System.Web.Http;
using Tw_Clone.Dto.Tweet;
using Tw_Clone.Models;
using Tw_Clone.Repositories;

namespace Tw_Clone.Services
{
    public class TweetService
    {


        private readonly ITweetRepository _tweetRepo;
        private readonly UserService _userService ;
        private readonly IMapper _mapper;
        private readonly TwcloneContext db; 
 

        public TweetService( ITweetRepository repository , IMapper mapper , TwcloneContext twcloneContext , UserService userService ) {
            _tweetRepo = repository;
            _mapper = mapper;
            db = twcloneContext;
            _userService = userService; 
        }

        public async Task<List<TweetsDto>> GetAll()
        {
            var lista = await _tweetRepo.GetAll();
            return _mapper.Map<List<TweetsDto>>(lista);
        }



        public async Task<List<TweetsDto>> GetAllByUserName(int userId)
        {
            var lista = await _tweetRepo.GetAll(u => u.UserId == userId);
            return _mapper.Map<List<TweetsDto>>(lista);
        }


        public async Task<TweetDto> GetById(int id)
        {
            var tw = await _tweetRepo.GetOne(tw => tw.Id == id);

            if (tw == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var user = await  _userService.GetById(tw.UserId); 
            
            var tweet =   _mapper.Map<TweetDto>(tw);

            tweet.UserName = user.Username;

            tweet.Comments = await  GetCommentsByTweetId(tw.Id);

            return tweet; 
        }

        private async Task<List<TweetsDto>> GetCommentsByTweetId(int id)
        {
            var comments = await _tweetRepo.GetAll(t => db.Comments.Any(c => c.TweetedId == id && c.TweetCommentId == t.Id));
            //  select t.* from comments c inner join tweets t on t.id = c.tweet_comment_id where c.tweeted_id = 4   
            return _mapper.Map<List<TweetsDto>>(comments);
        }

        public async Task<TweetDto> Create(CreateTweetDto createTweetDto)
        {
            var post = _mapper.Map<Tweet>(createTweetDto);

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
