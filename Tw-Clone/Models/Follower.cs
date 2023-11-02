using System;
using System.Collections.Generic;

namespace Tw_Clone.Models;

public partial class Follower
{
    public int FollowerId { get; set; }

    public int FollowingId { get; set; }

    public DateTime FollowedAt { get; set; }

    public DateTime? UnfollowedAt { get; set; }

    public virtual User FollowerNavigation { get; set; } = null!;

    public virtual User Following { get; set; } = null!;
}
