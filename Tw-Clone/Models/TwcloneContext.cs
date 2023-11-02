using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Tw_Clone.Models.TweetLike;
using Tw_Clone.Models.TweetRepost;

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

    public virtual DbSet<Tweet.Tweet> Tweets { get; set; }

    public virtual DbSet<Tweetslike> Tweetslikes { get; set; }

    public virtual DbSet<Tweetsrepost> Tweetsreposts { get; set; }

    public virtual DbSet<User.User> Users { get; set; }

  

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Tweet.Tweet>(entity =>
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

            entity.HasMany(d => d.TweetComments).WithMany(p => p.Tweeteds)
                .UsingEntity<Dictionary<string, object>>(
                    "Comment",
                    r => r.HasOne<Tweet.Tweet>().WithMany()
                        .HasForeignKey("TweetCommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("comments_ibfk_2"),
                    l => l.HasOne<Tweet.Tweet>().WithMany()
                        .HasForeignKey("TweetedId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("comments_ibfk_1"),
                    j =>
                    {
                        j.HasKey("TweetedId", "TweetCommentId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("comments");
                        j.HasIndex(new[] { "TweetCommentId" }, "tweet_comment_id");
                        j.IndexerProperty<int>("TweetedId").HasColumnName("tweeted_id");
                        j.IndexerProperty<int>("TweetCommentId").HasColumnName("tweet_comment_id");
                    });

            entity.HasMany(d => d.Tweeteds).WithMany(p => p.TweetComments)
                .UsingEntity<Dictionary<string, object>>(
                    "Comment",
                    r => r.HasOne<Tweet.Tweet>().WithMany()
                        .HasForeignKey("TweetedId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("comments_ibfk_1"),
                    l => l.HasOne<Tweet.Tweet>().WithMany()
                        .HasForeignKey("TweetCommentId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("comments_ibfk_2"),
                    j =>
                    {
                        j.HasKey("TweetedId", "TweetCommentId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("comments");
                        j.HasIndex(new[] { "TweetCommentId" }, "tweet_comment_id");
                        j.IndexerProperty<int>("TweetedId").HasColumnName("tweeted_id");
                        j.IndexerProperty<int>("TweetCommentId").HasColumnName("tweet_comment_id");
                    });
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

        modelBuilder.Entity<User.User>(entity =>
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

            entity.HasMany(d => d.Followers).WithMany(p => p.Followings)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User.User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_1"),
                    l => l.HasOne<User.User>().WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_2"),
                    j =>
                    {
                        j.HasKey("FollowerId", "FollowingId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("follower");
                        j.HasIndex(new[] { "FollowingId" }, "following_id");
                        j.IndexerProperty<int>("FollowerId").HasColumnName("follower_id");
                        j.IndexerProperty<int>("FollowingId").HasColumnName("following_id");
                    });

            entity.HasMany(d => d.Followings).WithMany(p => p.Followers)
                .UsingEntity<Dictionary<string, object>>(
                    "Follower",
                    r => r.HasOne<User.User>().WithMany()
                        .HasForeignKey("FollowingId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_2"),
                    l => l.HasOne<User.User>().WithMany()
                        .HasForeignKey("FollowerId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("follower_ibfk_1"),
                    j =>
                    {
                        j.HasKey("FollowerId", "FollowingId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("follower");
                        j.HasIndex(new[] { "FollowingId" }, "following_id");
                        j.IndexerProperty<int>("FollowerId").HasColumnName("follower_id");
                        j.IndexerProperty<int>("FollowingId").HasColumnName("following_id");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
