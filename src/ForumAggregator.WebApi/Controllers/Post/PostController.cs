namespace ForumAggregator.WebApi.Controllers.Post;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using FluentValidation;
using AutoMapper;

using ForumAggregator.Application.Services;
using ForumAggregator.Application.UseCases;
using ForumAggregator.Application;

[ApiController]
[Route("/")]
public class PostController: ControllerBase
{
    private readonly IPostUseCase _postUseCase;
    private readonly IValidator<AddPostRequest> _addPostRequestValidator;
    private readonly IValidator<UpdatePostRequest> _updatePostRequestValidator;
    private readonly IAppContext _appContext;
    private readonly ForumAggregator.Application.Services.IPostService _postService;
    private readonly IMapper _mapper;
    private readonly ForumAggregator.Application.Services.IForumService _forumAppService;

    public PostController(
        IPostUseCase postUseCase,
        IValidator<AddPostRequest> addPostRequestValidator,
        IValidator<UpdatePostRequest> updatePostRequestValidator,
        IAppContext appContext,
        ForumAggregator.Application.Services.IPostService postService,
        IMapper mapper,
        ForumAggregator.Application.Services.IForumService forumAppService
    )
    {
        _postUseCase = postUseCase;
        _addPostRequestValidator = addPostRequestValidator;
        _appContext = appContext;
        _updatePostRequestValidator = updatePostRequestValidator;
        _postService = postService;
        _mapper = mapper;
        _forumAppService = forumAppService;
    }

    [HttpPost("post")]
    [Authorize]
    public IActionResult AddPost(AddPostRequest addPostRequest)
    {
        var validationResult = _addPostRequestValidator.Validate(addPostRequest);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());
        
        var result = _postUseCase.Create(
            addPostRequest.ForumId, 
            _appContext.UserId, 
            addPostRequest.Title, 
            addPostRequest.Content
        );

        if (result.Value == false || result.EntityUseCaseDto == null)
            return UnprocessableEntity(result.Result);
        
        return Ok(result.EntityUseCaseDto);
    }

    [HttpPatch("post")]
    [Authorize]
    public IActionResult UpdatePost(UpdatePostRequest updatePostRequest)
    {
        var validationResult = _updatePostRequestValidator.Validate(updatePostRequest);
        if (validationResult.IsValid == false)
            return BadRequest(validationResult.ToString());
        
        var result = _postService.UpdatePost(
            updatePostRequest.PostId,
            updatePostRequest.Title,
            updatePostRequest.Content
        );

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok();
    }

    [HttpDelete("post/{postId}")]
    [Authorize]
    public IActionResult DeletePost(string postId)
    {
        Guid guidPostId;
        
        if (Guid.TryParse(postId, out guidPostId) == false)
            return NotFound(postId);
        
        var result = _postService.DeletePost(guidPostId);
        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok();
    }

    [HttpGet("post/{postId}")]
    [AllowAnonymous]
    public IActionResult ReadPost(string postId)
    {
        Guid guidPostId;
        
        if (Guid.TryParse(postId, out guidPostId) == false)
            return NotFound(postId);
        
        var post = _postService.GetPost(guidPostId);
        if (post == null)
            return NotFound(guidPostId);
        
        return Ok(_mapper.Map<ReadPostResponse>(post));
    }

    [HttpGet("post/forum/{forumId}")]
    [AllowAnonymous]
    public IActionResult ReadPostFromForum(string forumId)
    {
        Guid guidForumId;
        ICollection<PostAppServiceModel> posts = default!;

        if (Guid.TryParse(forumId, out guidForumId) == true)
        {
            posts = _postService.GetAllPostsFromForum(guidForumId);
        }
        else
        {
            var forum = _forumAppService.GetForumByName(forumId);
            if (forum == null)
                return NotFound(forumId);
            
            posts = _postService.GetAllPostsFromForum(forum.Id);
        }
        
        return Ok(_mapper.Map<ICollection<PostAppServiceModel>, ICollection<ReadPostResponse>>(posts));
    }

    [HttpGet("post/user/{userId}")]
    [AllowAnonymous]
    public IActionResult ReadPostFromUser(string userId)
    {
        Guid guidUserId;

        if (Guid.TryParse(userId, out guidUserId) == false)
        {
            return NotFound(userId);
        }
        
        return Ok(_mapper.Map<ICollection<PostAppServiceModel>, ICollection<ReadPostResponse>>(_postService.GetAllPostsFromUser(guidUserId)));
    }
}