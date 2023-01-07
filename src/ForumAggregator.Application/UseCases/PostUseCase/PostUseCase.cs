namespace ForumAggregator.Application.UseCases;

using System;

using ForumAggregator.Domain.Shared.Interfaces;

public class PostUseCase: IPostUseCase
{
    private readonly IPostRepository _postRepository;
    private readonly IForumRepository _forumRepository;

    public PostUseCase(
        IPostRepository postRepository,
        IForumRepository forumRepository
    )
    {
        _postRepository = postRepository;
        _forumRepository = forumRepository;
    }

    public EntityUseCaseResult Create(string title, string content, Guid userId)
    {
        throw new NotImplementedException();
    }
}