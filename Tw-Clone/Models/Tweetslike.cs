using System;
using System.Collections.Generic;

namespace Tw_Clone.Models;

public partial class Tweetslike
{
    public int UserId { get; set; }

    public int TweetId { get; set; }

    public DateTime FhLike { get; set; }

    public virtual Tweet Tweet { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
