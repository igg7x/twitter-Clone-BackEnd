using System.Net;
using System.Web.Http;
using Tw_Clone.Dto.User;
using Tw_Clone.Models;
using Tw_Clone.Repositories;
using AutoMapper;

namespace Tw_Clone.Services
{
    public class UserService
    {

        private readonly IUserRepository _userRepo;
        private readonly IEncoderService _encoderService;
        private readonly TweetService _tweetService;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepo, TweetService tweetService, IEncoderService encoderService, IMapper mapper) {

            _userRepo = userRepo;
            _tweetService = tweetService;
            _encoderService = encoderService;
            _mapper = mapper;
        }


        

        public async Task<UserDto> GetById(int id)
        {
            var user = await _userRepo.GetOne(u => u.Id == id);

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return _mapper.Map<UserDto>(user);
        }

        public async Task<UserDto> GetUserByUsername(string? username) {

            var user = await _userRepo.GetOne(u => u.Username == username);
            
            if (user == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            
            var tweets = await _tweetService.GetAllByUserName(user.Id);

            var likes = await  _tweetService.GetAllLikesByUserName(user.Id);


            var reposts = await  _tweetService.GetAllRepostsByUserName(user.Id);

            var mapped = _mapper.Map<UserDto>(user);

            mapped.Tweets = tweets;

            mapped.Likes = likes; 

            mapped.Reposts= reposts;

            return mapped;
        }

       

        public async Task<UserDto> Create(CreateUserDto userToCreate) {

            User user = _mapper.Map<User>(userToCreate);
            user.Password = _encoderService.Encode(userToCreate.Password);
            await _userRepo.Add(user);
            return _mapper.Map<UserDto>(user);

        }


        public async Task<UserDto> UpdateByUsername(string? username , UpdateUserDto updateUserDto) {
        
            User user = await _userRepo.GetOne(u => u.Username == username);
            if(user == null) throw new HttpResponseException(HttpStatusCode.NotFound);
            var updated = _mapper.Map(updateUserDto, user);
            return _mapper.Map<UserDto>(await _userRepo.Update(updated));
        }


        public async Task Delete(string username) {

            var user = await _userRepo.GetOne(u => u.Username == username);

            if (user == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            await _userRepo.Delete(user);

        }

    }
}
