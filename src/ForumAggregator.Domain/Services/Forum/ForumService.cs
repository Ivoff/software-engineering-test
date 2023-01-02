namespace ForumAggregator.Domain.Services.ForumService;

using System;

// This is a Domain Service because is a business rule.
public class ForumService : IForumService
{
    public bool IsForumNameUnique(string forumName)
    {
        return false;
    }
}