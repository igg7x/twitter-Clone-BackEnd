﻿using System;
using System.Collections.Generic;

namespace Tw_Clone.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Biography { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? FhNac { get; set; }

    public string Password { get; set; } = null!;

    public int? NumFollowers { get; set; }

    public int? NumFollowing { get; set; }

    public DateTime? DeletedAt { get; set; }

    public sbyte? Banned { get; set; }

    public virtual ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();

    public virtual ICollection<Tweetslike> Tweetslikes { get; set; } = new List<Tweetslike>();

    public virtual ICollection<Tweetsrepost> Tweetsreposts { get; set; } = new List<Tweetsrepost>();

    public virtual ICollection<User> Followers { get; set; } = new List<User>();

    public virtual ICollection<User> Followings { get; set; } = new List<User>();
}