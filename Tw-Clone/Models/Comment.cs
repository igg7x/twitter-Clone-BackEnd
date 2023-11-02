using System;
using System.Collections.Generic;

namespace Tw_Clone.Models;

public partial class Comment
{
    public int TweetedId { get; set; }

    public int TweetCommentId { get; set; }

    public sbyte? Banned { get; set; }

    public virtual Tweet TweetComment { get; set; } = null!;

    public virtual Tweet Tweeted { get; set; } = null!;
}
