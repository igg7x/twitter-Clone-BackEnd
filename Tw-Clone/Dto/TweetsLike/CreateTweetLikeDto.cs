using System.ComponentModel.DataAnnotations;

namespace Tw_Clone.Dto.TweetsLike
{
    public class CreateTweetLikeDto
    {
        [Required]
        public string UserName { get; set; } = null!;
        [Required]
        public int  TweetId { get; set;}
    }
}
