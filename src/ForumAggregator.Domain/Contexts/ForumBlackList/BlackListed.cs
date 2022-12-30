namespace ForumAggregator.Domain.ForumBlackList;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

public class BlackListed: IEntity
{
    // Fields & Properties

    public Guid Id { get; init; }

    public Guid UserId { get; init; }

    public bool CanPost { get; private set; } = default!;

    public bool CanComment { get; private set; } = default!;

    public bool Deleted { get; private set; } = default!;

    // Constructors

    private BlackListed () {}

    public BlackListed (Guid userId, bool canPost, bool canComment)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CanPost = canPost;
        CanComment = canComment;
        Deleted = false;
    }

    // Methods

    public BlackListedResult UpdateCanPost (bool value)
    {
        if (Deleted)
            return DeletedResult();
        
        CanPost = value;
        
        return new BlackListedResult()
        {
            Value = true,
            Result = string.Empty
        };
    }

    public BlackListedResult UpdateCanComment (bool value)
    {
        if (Deleted)
            return DeletedResult();
        
        CanComment = value;

        return new BlackListedResult()
        {
            Value = true,
            Result = string.Empty
        };
    }

    public BlackListedResult Delete ()
    {
        if (Deleted)
            return DeletedResult();

        Deleted = true;
        return new BlackListedResult()
        {
            Value = true,
            Result = string.Empty
        };
    }

    private BlackListedResult DeletedResult()
    {
        return new BlackListedResult()
        {
            Value = false,
            Result = "User restrictions have been removed."
        };
    }

    public static BlackListed Load (Guid blackListedId, Guid userId, bool canPost, bool canComment, bool deleted)
    {
        return new BlackListed()
        {
            Id = blackListedId,
            UserId = userId,
            CanPost = canPost,
            CanComment = canComment,
            Deleted = deleted
        };
    }
}