namespace ForumAggregator.Domain.CommentRegistry;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class Comment: IEntity
{
    // Fields & Properties

    public Guid Id { get; init; }

    public Guid PostId { get; init; }

    public Guid? ParentCommentId { get; init; }
    
    public CommentAuthor Author { get; init; } = default!;

    public string Content { get; private set; } = default!;    

    public bool Deleted {get; private set; } = default!;

    // Constructors

    private Comment (){}

    private Comment (Guid postId, Guid? parentCommentId, string content, CommentAuthor author)
    {
        Id = Guid.NewGuid();
        PostId = postId;
        ParentCommentId = parentCommentId;
        Content = content;
        Author = author;
        Deleted = false;
    }

    // Methods

    public CommentResult UpdateComment(Guid actor, string content)
    {
        if (Deleted)
            return DeletedResult();

        if (actor == Author.Id)
        {
            Content = content;
            return new CommentResult()
            {
                Value = true,
                Result = "Comment successfully updated.",
                Comment = null
            };
        }

        return new CommentResult()
        {
            Value = false,
            Result = "User is not the author of the Comment.",
            Comment = null
        };
    }

    public CommentResult RemoveComment(Guid actor)
    {
        if (Deleted)
            return DeletedResult();

        if (actor == Author.Id)
        {
            return new CommentResult()
            {
                Value = true,
                Result = string.Empty,
                Comment = null
            };
        }

        return new CommentResult()
        {
            Value = false,
            Result = "User is not the author of the comment.",
            Comment = null
        };
    }

    public CommentResult Delete()
    {
        if (Deleted)
            return DeletedResult();

        Deleted = true;
        return new CommentResult()
        {
            Value = true,
            Result = string.Empty
        };
    }

    private CommentResult DeletedResult ()
    {
        return new CommentResult()
        {
            Value = false,
            Result = "Comment has been removed.",
            Comment = null
        };
    }

    public static CommentResult Create(Guid postId, Guid? parentCommentId, string content, CommentAuthor author)
    {
        Comment newComment = new Comment(postId, parentCommentId, content, author);
        bool resultValue = !author.CannotComment;
        return new CommentResult()
        {
            Value = resultValue,
            Result = resultValue ? "Comment successfully created" : "Comment has been blocked from being created",
            Comment = resultValue ? newComment : null
        };
    }
    
    public static Comment Load (Guid commentId, Guid postId, Guid? parentCommentId, string content, CommentAuthor author, bool deleted)
    {
        return new Comment()
        {
            Id = commentId,
            PostId = postId,
            ParentCommentId = parentCommentId,
            Content = content,
            Author = author,
            Deleted = deleted
        };        
    }
}