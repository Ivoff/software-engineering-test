namespace ForumAggregator.Application.UseCases;

using System;

public interface IPostUseCase
{
    public EntityUseCaseResult Create(string title, string content, Guid userId);
}