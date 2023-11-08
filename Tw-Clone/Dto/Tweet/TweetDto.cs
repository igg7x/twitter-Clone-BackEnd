using Tw_Clone.Dto.User;

namespace Tw_Clone.Dto.Tweet
{
    public class TweetDto
    {
        public int Id { get; set; }

        public string UserName { get; set; } = null!; 

        public string TweetText { get; set; } = null!;

        public int? NumLikes { get; set; }

        public int? NumReposts { get; set; }

        public int? NumComments { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<TweetsDto>? Comments  { get; set; }

        //public List<UsersDto>? Likes { get; set;  }
        //public List<UsersDto>? Reposts { get; set; }


    }
}
