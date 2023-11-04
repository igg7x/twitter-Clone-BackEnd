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
        private readonly IMapper _mapper;



        public TweetService(IMapper mapper, ITweetRepository repository) {
            _tweetRepo = repository;
            _mapper = mapper;
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
            var post = await _tweetRepo.GetOne(u => u.Id == id);

            if (post == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return _mapper.Map<TweetDto>(post);
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


        public async Task DeleteById(int id)
        {
            var tweet = await _tweetRepo.GetOne(u => u.Id == id);

            if (tweet == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

          await _tweetRepo.Delete(p);
        }
    }
}
