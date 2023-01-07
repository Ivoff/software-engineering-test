namespace ForumAggregator.Domain.Shared.Interfaces;

using System;
using System.Collections.Generic;

public interface IForumRepository
{
    public Domain.ForumRegistry.Forum? Get(Guid forumId);
    public Domain.ForumRegistry.Forum? GetByName(string forumName);
    public ICollection<Domain.ForumRegistry.Forum> GetAll();
    public bool Save(Domain.ForumRegistry.Forum forum);
    public bool SaveModerator(Guid forumId, Domain.ForumRegistry.Moderator moderator);
    public bool SaveBlackListed(Guid forumId, Domain.ForumRegistry.BlackListed blackListed);
    // public bool UpdateForum(Domain.ForumRegistry.Forum forum);
    // public bool AddModerator(Domain.ForumRegistry.Forum forum);
    // public bool UpdateModerator(Domain.ForumRegistry.Forum forum);
    // public bool AddBlackListed(Domain.ForumRegistry.Forum forum);
    // public bool UpdateBlackListed(Domain.ForumRegistry.Forum forum);
}