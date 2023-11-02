using Tw_Clone.Models.User.Dto;
using Tw_Clone.Models.User;
using AutoMapper;
using Microsoft.Extensions.Hosting;
using Tw_Clone.Models.Tweet;
using Tw_Clone.Models.Tweet.Dto;
using Tw_Clone.Models.TweetLike;
using Tw_Clone.Models.TweetLike.Dto;
using Tw_Clone.Models.TweetRepost;
using Tw_Clone.Models.TweetRepost.Dto;

namespace Tw_Clone.Config
{
    public class Mapping : Profile
    {
        public Mapping() {

            //user
            CreateMap<User, UsersDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            CreateMap<UpdateUserDto, User>().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // tweet
            CreateMap<Tweet, TweetsDto>().ReverseMap();
            CreateMap<Tweet, TweetDto>().ReverseMap();
            CreateMap<CreateTweetDto, Tweet>().ReverseMap();
            CreateMap<UpdateTweetDto, Tweet>().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            //tweet Like
            CreateMap<Tweetslike , TweetsLikeDto>().ReverseMap();

            //tweet Repost
            CreateMap<Tweetsrepost, TweetsRepostDto>().ReverseMap();




        }

        
    }
}
