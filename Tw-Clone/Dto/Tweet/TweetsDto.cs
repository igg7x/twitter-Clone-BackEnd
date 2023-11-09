namespace Tw_Clone.Dto.Tweet
{
    public class TweetsDto
    {

        public int Id { get; set;}
        public int UserId { get; set; }
        // USERName
        // IMAGE 
        // firstname 
        // lastname

        public string UserName { get; set; } = null!;

        public string FirstName { get; set; } = null!;

        public string LastName { get; set; } = null!;

        public string Image { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        public string TweetText { get; set; } = null!;

        public int? NumLikes { get; set; }

        public int? NumReposts { get; set; }

        public int? NumComments { get; set; }
    }
}
