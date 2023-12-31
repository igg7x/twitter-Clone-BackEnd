﻿using AutoMapper;
using Tw_Clone.Dto.Tweet;
using Tw_Clone.Dto.TweetsLike;
using Tw_Clone.Dto.TweetsReposts;
using Tw_Clone.Dto.User;
using Tw_Clone.Models;
namespace Tw_Clone.Config
{
    public class Mapping   :Profile
    {

        public Mapping()
        {
            CreateMap<User, UsersDto>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, VerifyUserDto>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();
            // no mapear los null en el update
            CreateMap<UpdateUserDto, User>().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

            // Post
            CreateMap<Tweet, TweetsDto>().ReverseMap();
            CreateMap<Tweet, TweetDto>().ReverseMap();
            CreateMap<TweetDto ,Comment>().ReverseMap();
            CreateMap<Comment ,TweetDto>().ReverseMap();
            CreateMap<CreateTweetDto, Tweet>().ReverseMap();
            CreateMap<CreateTweetLikeDto, Tweetslike>().ReverseMap();
            CreateMap<CreateTweetRepostDto, Tweetsrepost>().ReverseMap();
            // no mapear los null en el update
            CreateMap<UpdateTweetDto, Tweet>().ForAllMembers(opts => opts.Condition((_, _, srcMember) => srcMember != null));

        }

    }
}
