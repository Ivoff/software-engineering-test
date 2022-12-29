namespace ForumAggregator.Domain.CommentRegistry;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class Comment: IEntity
{
    // Fields & Properties

    private Guid _comment_id;
    public Guid Id 
    { 
        get => _comment_id; 
        init => _comment_id = value;
    }

    public Guid PostId { get; init; }

    public Guid? ParentCommentId { get; init; }
    
    public CommentAuthor Author { get; init; } = default!;

    public string Content { get; private set; } = default!;    

    // Constructors

    private Comment (){}

    private Comment (Guid postId, Guid? parentCommentId, string content, CommentAuthor author)
    {
        _comment_id = Guid.NewGuid();
        PostId = postId;
        ParentCommentId = parentCommentId;
        Content = content;
        Author = author;
    }

    // Methods

    public CommentServiceResult UpdateComment(Guid actor, string content)
    {
        if (actor == Author.Id)
        {
            Content = content;
            return new CommentServiceResult()
            {
                Value = true,
                Result = "Comment successfully updated.",
                Comment = null
            };
        }

        return new CommentServiceResult()
        {
            Value = false,
            Result = "User is not the author of the comment.",
            Comment = null
        };
    }

    public CommentServiceResult RemoveComment(Guid actor)
    {
        if (actor == Author.Id)
        {
            return new CommentServiceResult()
            {
                Value = true,
                Result = string.Empty,
                Comment = null
            };
        }

        return new CommentServiceResult()
        {
            Value = false,
            Result = "User is not the author of the comment.",
            Comment = null
        };
    }

    public static CommentServiceResult Create(Guid postId, Guid? parentCommentId, string content, CommentAuthor author)
    {
        Comment newComment = new Comment(postId, parentCommentId, content, author);
        bool resultValue = !author.AuthorCannotComment;
        return new CommentServiceResult()
        {
            Value = resultValue,
            Result = resultValue ? "Comment successfully created" : "Comment has been blocked from being created",
            Comment = resultValue ? newComment : null
        };
    }
    
    public static Comment Load (Guid commentId, Guid postId, Guid? parentCommentId, string content, CommentAuthor author)
    {
        return new Comment()
        {
            Id = commentId,
            PostId = postId,
            ParentCommentId = parentCommentId,
            Content = content,
            Author = author
        };        
    }
}