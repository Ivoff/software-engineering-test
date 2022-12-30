namespace ForumAggregator.Domain.ForumBlackList;

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

    public void Update (Guid userId, bool canComment, bool canPost)
    {
        BlackListed blackListed = BlackList.First(x => x.UserId == userId && x.Deleted == false);
        BlackList.Remove(blackListed);

        blackListed.UpdateCanComment(canComment);
        blackListed.UpdateCanPost(canPost);

        BlackList.Add(blackListed);
    }

    public static BlackListedCollection Load (ICollection<BlackListed> blacklist)
    {
        return new BlackListedCollection()
        {
            BlackList = blacklist
        };
    }
}