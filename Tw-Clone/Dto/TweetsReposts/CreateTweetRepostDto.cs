using System.ComponentModel.DataAnnotations;

namespace Tw_Clone.Dto.TweetsReposts
{
    public class CreateTweetRepostDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public int TweetId { get; set;  }

    }
}
