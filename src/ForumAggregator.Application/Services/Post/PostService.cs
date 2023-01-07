namespace ForumAggregator.Application.Services;

using AutoMapper;
using System;

using ForumAggregator.Domain.PostRegistry;
using ForumAggregator.Domain.Shared.Interfaces;

public class PostService : IPostService
{
    private readonly IMapper _mapper;
    private readonly IPostRepository _postRepository;
    private readonly IAppContext _appContext;

    public PostService(IMapper mapper, IPostRepository postRepository, IAppContext appContext)
    {
        _mapper = mapper;
        _postRepository = postRepository;
        _appContext = appContext;
    }

    public PostAppServiceModel? GetPost(Guid postId)
    {
        var post = _postRepository.Get(postId);
        if(post == null)
            return null;
        
        return _mapper.Map<PostAppServiceModel>(post);
    }

    public ServiceResult DeletePost(Guid postId)
    {
        var post = _postRepository.Get(postId);
        if(post == null)
            return new ServiceResult(false, $"Post {postId} does not exist.");
        
        var resultRemove = post.Remove(_appContext.UserId);

        if (resultRemove.Value == false)
            return new ServiceResult(false, resultRemove.Result);

        var result = _postRepository.Save(post);
        
        return new ServiceResult(
            result, 
            result ? string.Empty : "Something wrong happened during data persistance"
        );
    }

    public ServiceResult UpdatePost(Guid postId, string newTitle, string newContent)
    {
        var domainPost = _postRepository.Get(postId);
        if (domainPost == null)
            return new ServiceResult(false, $"Post {postId} does not exist.");
        
        domainPost.Update(
            _appContext.UserId,
            string.IsNullOrEmpty(newTitle) ? domainPost.Title : newTitle,
            string.IsNullOrEmpty(newContent) ? domainPost.Content : newContent
        );

        var result = _postRepository.Save(domainPost);
        return new ServiceResult(
            result,
            result ? string.Empty : "Something wrong happened during data persistance"
        );
    }
}