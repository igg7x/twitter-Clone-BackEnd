namespace Tw_Clone.Models.Tweet.Dto
{
    public class TweetsDto
    {
        public int UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string TweetText { get; set; } = null!;

        public int? NumLikes { get; set; }

        public int? NumReposts { get; set; }

        public int? NumComments { get; set; }
    }
}
