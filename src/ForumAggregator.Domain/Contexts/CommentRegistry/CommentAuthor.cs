namespace ForumAggregator.Domain.CommentRegistry;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class CommentAuthor: IEntity
{
    // Fields & Properties

    public Guid Id { get; init; }

    public bool CannotComment { get; init; }

    // Constructors

    private CommentAuthor (){}

    public CommentAuthor (Guid authorId, bool authorCannotComment)
    {
        Id = authorId;
        CannotComment = authorCannotComment;
    }
}