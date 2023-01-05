namespace ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;

public interface IForumRepository
{
    public Domain.ForumRegistry.Forum? Get(Guid forumId);
    public Domain.ForumRegistry.Forum? GetByName(string forumName);
    public bool Save(Domain.ForumRegistry.Forum forum);
}