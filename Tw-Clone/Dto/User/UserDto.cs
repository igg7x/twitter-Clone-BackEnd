using Tw_Clone.Dto.Tweet;
using Tw_Clone.Models;

namespace Tw_Clone.Dto.User
{
    public class UserDto
    {
        public int Id { get; set; }
       
        public string Username { get; set; } = null!;

        public DateTime? FhNac { get; set; }

        public string? Biography { get; set; }
        public int? Num_Followers { get; set; }

        public int? Num_Following { get; set; }

        public List<TweetsDto>? Tweets { get; set; }

        public List<TweetsDto> ?Likes { get; set; }

        public List<TweetsDto>?  Reposts { get; set; }

    }
}
