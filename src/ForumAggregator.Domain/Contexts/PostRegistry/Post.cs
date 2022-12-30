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

    public bool Deleted { get; private set; } = default!;

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

    public PostResult UpdatePost (Guid actor, string newTitle, string newContent)
    {
        if (Deleted)
            return DeletedResult();

        if (actor == Author.Id)
        {
            Title = newTitle;
            Content = newContent;
            return new PostResult()
            {
                Value = true,
                Result = "Post successfully updated.",
                Post = null
            };
        }

        return new PostResult()
        {
            Value = false,
            Result = "User is not the Author of the Post.",
            Post = null
        };
    }

    public PostResult RemovePost (Guid actor)
    {
        if (Deleted)
            return DeletedResult();

        if (actor == Author.Id)
        {
            return new PostResult()
            {
                Value = true,
                Result = string.Empty,
                Post = null
            };
        }

        return new PostResult()
        {
            Value = false,
            Result = "User is not the Author of the Post",
            Post = null
        };
    }

    public PostResult Delete()
    {
        if (Deleted)
            return DeletedResult();

        Deleted = true;
        return new PostResult()
        {
            Value = true,
            Result = string.Empty,
            Post = null
        };
    }

    private PostResult DeletedResult ()
    {
        return new PostResult()
        {
            Value = false,
            Result = "Post has been removed.",
            Post = null
        };
    }

    public PostResult Create (Guid forumId, string title, string content, PostAuthor author)
    {
        Post newPost = new Post(forumId, title, content, author);
        bool resultValue = !author.CannotPost;
        return new PostResult()
        {
            Value = resultValue,
            Result = resultValue ? "Post successfully created" : "Post has been blocked from being created",
            Post = resultValue ? newPost : null
        };
    }

    public static Post Load (Guid postId, Guid forumId, string title, string content, bool deleted, PostAuthor author)
    {
        return new Post()
        {
            Id = postId,
            ForumId = forumId,
            Title = title,
            Content = content,
            Author = author,
            Deleted = deleted
        };
    }
}