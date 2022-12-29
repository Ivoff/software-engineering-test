namespace ForumAggregator.Domain.CommentRegistry;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class CommentAuthor: IEntity
{
    // Fields & Properties

    private Guid _author_id;
    public Guid Id 
    { 
        get => _author_id; 
        init => _author_id = value;
    }

    public bool AuthorCannotComment { get; init; }

    // Constructors

    private CommentAuthor (){}

    public CommentAuthor (Guid authorId, bool authorCannotComment)
    {
        _author_id = authorId;
        AuthorCannotComment = authorCannotComment;
    }
}