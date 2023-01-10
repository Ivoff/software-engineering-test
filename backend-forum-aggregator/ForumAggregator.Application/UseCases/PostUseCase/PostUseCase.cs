namespace ForumAggregator.Application.UseCases;

using System;

using ForumAggregator.Domain.Shared.Interfaces;
using ForumAggregator.Domain.PostRegistry;

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

    public EntityUseCaseResult Create(Guid forumId, Guid authorUserId, string title, string content)
    {
        var forum = _forumRepository.Get(forumId);
        if (forum == null)
            return new EntityUseCaseResult(false, $"Forum {forumId} does not exist.", null);
        
        
        var blackListedExit = forum.GetBlackListedByUserId(authorUserId);
        var postAuthor = new PostAuthor(authorUserId, !blackListedExit?.CanPost ?? false);
        var postCreationResult = Post.Create(forumId, title, content, postAuthor);
        
        if (postCreationResult.Value == false)
        {
            return new EntityUseCaseResult(false, postCreationResult.Result, null);
        }

        Post post = postCreationResult.Post!;

        var result = _postRepository.Save(post);

        return new EntityUseCaseResult(
            result,
            result ? string.Empty : "Something wrong happened during data persistance",
            result ? new EntityUseCaseDto(post.Id, post.Title) : null
        );
    }
}