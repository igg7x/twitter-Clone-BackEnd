using System;
using System.Collections.Generic;

namespace Tw_Clone.Models;

public partial class Tweet
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string TweetText { get; set; } = null!;

    public int? NumLikes { get; set; }

    public int? NumReposts { get; set; }

    public int? NumComments { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Comment> CommentTweetComments { get; set; } = new List<Comment>();

    public virtual ICollection<Comment> CommentTweeteds { get; set; } = new List<Comment>();

    public virtual ICollection<Tweetslike> Tweetslikes { get; set; } = new List<Tweetslike>();

    public virtual ICollection<Tweetsrepost> Tweetsreposts { get; set; } = new List<Tweetsrepost>();

    public virtual User User { get; set; } = null!;
}
