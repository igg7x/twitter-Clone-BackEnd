using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Tw_Clone.Models;

public partial class TwcloneContext : DbContext
{
    public TwcloneContext()
    {
    }

    public TwcloneContext(DbContextOptions<TwcloneContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Follower> Followers { get; set; }

    public virtual DbSet<Tweet> Tweets { get; set; }

    public virtual DbSet<Tweetslike> Tweetslikes { get; set; }

    public virtual DbSet<Tweetsrepost> Tweetsreposts { get; set; }

    public virtual DbSet<User> Users { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseMySql("server=localhost;database=twclone;uid=root;pwd=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => new { e.TweetedId, e.TweetCommentId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("comments");

            entity.HasIndex(e => e.TweetCommentId, "tweet_comment_id");

            entity.Property(e => e.TweetedId).HasColumnName("tweeted_id");
            entity.Property(e => e.TweetCommentId).HasColumnName("tweet_comment_id");
            entity.Property(e => e.Banned)
                .HasDefaultValueSql("'0'")
                .HasColumnName("banned");

            entity.HasOne(d => d.TweetComment).WithMany(p => p.CommentTweetComments)
                .HasForeignKey(d => d.TweetCommentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_ibfk_2");

            entity.HasOne(d => d.Tweeted).WithMany(p => p.CommentTweeteds)
                .HasForeignKey(d => d.TweetedId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comments_ibfk_1");
        });

        modelBuilder.Entity<Follower>(entity =>
        {
            entity.HasKey(e => new { e.FollowerId, e.FollowingId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("follower");

            entity.HasIndex(e => e.FollowingId, "following_id");

            entity.Property(e => e.FollowerId).HasColumnName("follower_id");
            entity.Property(e => e.FollowingId).HasColumnName("following_id");
            entity.Property(e => e.FollowedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp")
                .HasColumnName("followed_at");
            entity.Property(e => e.UnfollowedAt)
                .HasColumnType("datetime")
                .HasColumnName("unfollowed_at");

            entity.HasOne(d => d.FollowerNavigation).WithMany(p => p.FollowerFollowerNavigations)
                .HasForeignKey(d => d.FollowerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("follower_ibfk_1");

            entity.HasOne(d => d.Following).WithMany(p => p.FollowerFollowings)
                .HasForeignKey(d => d.FollowingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("follower_ibfk_2");
        });

        modelBuilder.Entity<Tweet>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("tweets");

            entity.HasIndex(e => e.UserId, "user_id");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.NumComments)
                .HasDefaultValueSql("'0'")
                .HasColumnName("num_comments");
            entity.Property(e => e.NumLikes)
                .HasDefaultValueSql("'0'")
                .HasColumnName("num_likes");
            entity.Property(e => e.NumReposts)
                .HasDefaultValueSql("'0'")
                .HasColumnName("num_reposts");
            entity.Property(e => e.TweetText)
                .HasMaxLength(256)
                .HasColumnName("tweet_text");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Tweets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tweets_ibfk_1");
        });

        modelBuilder.Entity<Tweetslike>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TweetId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("tweetslikes");

            entity.HasIndex(e => e.TweetId, "tweet_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TweetId).HasColumnName("tweet_id");
            entity.Property(e => e.FhLike)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp")
                .HasColumnName("fh_like");

            entity.HasOne(d => d.Tweet).WithMany(p => p.Tweetslikes)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tweetslikes_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Tweetslikes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tweetslikes_ibfk_1");
        });

        modelBuilder.Entity<Tweetsrepost>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TweetId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("tweetsreposts");

            entity.HasIndex(e => e.TweetId, "tweet_id");

            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.TweetId).HasColumnName("tweet_id");
            entity.Property(e => e.FhReposts)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp")
                .HasColumnName("fh_reposts");

            entity.HasOne(d => d.Tweet).WithMany(p => p.Tweetsreposts)
                .HasForeignKey(d => d.TweetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tweetsreposts_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Tweetsreposts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tweetsreposts_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.HasIndex(e => e.Username, "username").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Banned)
                .HasDefaultValueSql("'0'")
                .HasColumnName("banned");
            entity.Property(e => e.Biography)
                .HasMaxLength(128)
                .HasColumnName("biography");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp")
                .HasColumnName("created_at");
            entity.Property(e => e.DeletedAt)
                .HasColumnType("datetime")
                .HasColumnName("deleted_at");
            entity.Property(e => e.Email)
                .HasMaxLength(30)
                .HasColumnName("email");
            entity.Property(e => e.FhNac)
                .HasColumnType("datetime")
                .HasColumnName("Fh_nac");
            entity.Property(e => e.FirstName)
                .HasMaxLength(40)
                .HasColumnName("first_name");
            entity.Property(e => e.Image)
                .HasMaxLength(128)
                .HasColumnName("image");
            entity.Property(e => e.LastName)
                .HasMaxLength(40)
                .HasColumnName("last_name");
            entity.Property(e => e.NumFollowers)
                .HasDefaultValueSql("'0'")
                .HasColumnName("num_followers");
            entity.Property(e => e.NumFollowing)
                .HasDefaultValueSql("'0'")
                .HasColumnName("num_following");
            entity.Property(e => e.Password)
                .HasMaxLength(128)
                .HasColumnName("password");
            entity.Property(e => e.Username)
                .HasMaxLength(15)
                .HasColumnName("username");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
