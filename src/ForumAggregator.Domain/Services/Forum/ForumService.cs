namespace ForumAggregator.Domain.Services.ForumService;

using System;
using ForumAggregator.Domain.Shared.Interfaces;

// This is a Domain Service because is a business rule.
public class ForumService : IForumService
{
    private readonly IForumRepository _forumRepository;

    public ForumService(IForumRepository forumRepository)
    {
        _forumRepository = forumRepository;
    }

    public bool IsForumNameUnique(string forumName)
    {
        return _forumRepository.GetByName(forumName) != null;
    }
}