using System.ComponentModel.DataAnnotations;

namespace Tw_Clone.Dto.Tweet
{
    public class CreateTweetDto
    {

        [Required]
        public string UserName { get; set; } = null!; 
        [Required]
        [MaxLength(256)]
        public string TweetText { get; set; } = null!;
    }
}
