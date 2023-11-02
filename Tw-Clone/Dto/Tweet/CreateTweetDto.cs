﻿using System.ComponentModel.DataAnnotations;

namespace Tw_Clone.Dto.Tweet
{
    public class CreateTweetDto
    {

        [Required]
        public int UsertId { get; set; }
        [Required]
        [MaxLength(256)]
        public string TweetText { get; set; } = null!;
    }
}