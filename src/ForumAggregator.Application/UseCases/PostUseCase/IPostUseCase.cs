namespace ForumAggregator.Application.UseCases;

using System;

public interface IPostUseCase
{
    public EntityUseCaseResult Create(Guid forumId, Guid authorUserId, string title, string content);
}