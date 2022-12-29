namespace ForumAggregator.Domain.PostRegistry;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class Post : IEntity
{
    // Fields & Properties

    public Guid Id { get; init; }

    public Guid ForumId { get; init; }

    public string Title  { get; private set; } = default!;

    public string Content { get; private set; } = default!;

    public PostAuthor Author { get; init; } = default!;

    // Constructors

    private Post () {}

    private Post (Guid forumId, string title, string content, PostAuthor author)
    {
        Id = Guid.NewGuid();
        ForumId = forumId;
        Title = title;
        Content = content;
        Author = author;
    }

    // Methods

    public PostServiceResult UpdatePost (Guid actor, string newTitle, string newContent)
    {
        if (actor == Author.Id)
        {
            Title = newTitle;
            Content = newContent;
            return new PostServiceResult()
            {
                Value = true,
                Result = "Post successfully updated.",
                Post = null
            };
        }

        return new PostServiceResult()
        {
            Value = false,
            Result = "User is not the Author of the Post.",
            Post = null
        };
    }

    public PostServiceResult RemovePost (Guid actor)
    {
        if (actor == Author.Id)
        {
            return new PostServiceResult()
            {
                Value = true,
                Result = string.Empty,
                Post = null
            };
        }

        return new PostServiceResult()
        {
            Value = false,
            Result = "User is not the Author of the Post",
            Post = null
        };
    }

    public PostServiceResult Create (Guid forumId, string title, string content, PostAuthor author)
    {
        Post newPost = new Post(forumId, title, content, author);
        bool resultValue = !author.CannotPost;
        return new PostServiceResult()
        {
            Value = resultValue,
            Result = resultValue ? "Post successfully created" : "Post has been blocked from being created",
            Post = resultValue ? newPost : null
        };
    }

    public static Post Load (Guid postId, Guid forumId, string title, string content, PostAuthor author)
    {
        return new Post()
        {
            Id = postId,
            ForumId = forumId,
            Title = title,
            Content = content,
            Author = author
        };
    }
}