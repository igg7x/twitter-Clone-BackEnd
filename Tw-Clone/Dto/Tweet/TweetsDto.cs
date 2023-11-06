namespace Tw_Clone.Dto.Tweet
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
