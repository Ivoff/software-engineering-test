namespace ForumAggregator.Domain.PostRegistry;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class PostAuthor: IEntity
{
    // Fields & Properties

    public Guid Id { get; init; }

    public bool CannotPost { get; init; }

    // Constructors

    private PostAuthor () {}

    public PostAuthor (Guid authorId, bool cannotPost)
    {
        Id = authorId;
        CannotPost = cannotPost;
    }
}