namespace ForumAggregator.Domain.ForumRegistry;

using System;
using System.Collections.Generic;
using System.Linq;

public class BlackListedCollection
{
    // Fields & Properties

    public ICollection<BlackListed> BlackList { get; init; }

    // Constructors

    public BlackListedCollection ()
    {
        BlackList = new List<BlackListed>();
    }

    // Methods

    public BlackListed? GetByUserId (Guid userId)
    {
        return BlackList.FirstOrDefault(x => x.UserId == userId && x.Deleted == false);
    }

    public void Add (BlackListed newBlackListed)
    {
        BlackList.Add(newBlackListed);
    }

    public BlackListedResult Remove (BlackListed deletedBlackListed)
    {
        BlackListedResult deletionResult = deletedBlackListed.Delete();
        return new BlackListedResult()
        {
            Value = BlackList.Remove(deletedBlackListed) && deletionResult.Value,
            Result = deletionResult.Result
        };
    }

    public BlackListedResult Update (Guid userId, bool canComment, bool canPost)
    {
        BlackListed blackListed = BlackList.First(x => x.UserId == userId && x.Deleted == false);
        bool removed = BlackList.Remove(blackListed);

        BlackListedResult updateCommentResult = blackListed.UpdateCanComment(canComment);
        BlackListedResult updatePostResult = blackListed.UpdateCanPost(canPost);

        BlackList.Add(blackListed);

        return new BlackListedResult()
        {
            Value = removed && updateCommentResult.Value && updatePostResult.Value,
            Result = string.IsNullOrWhiteSpace(updateCommentResult.Result) ? updatePostResult.Result : updateCommentResult.Result
        };
    }

    public static BlackListedCollection Load (ICollection<BlackListed> blacklist)
    {
        return new BlackListedCollection()
        {
            BlackList = blacklist
        };
    }
}