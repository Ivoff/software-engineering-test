namespace ForumAggregator.WebApi.Controllers.Forum;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using FluentValidation;
using FluentValidation.Results;
using AutoMapper;

using ForumAggregator.Application.UseCases;
using ForumAggregator.Application.Services;

[ApiController]
[Route("/")]
public class ForumController: ControllerBase
{
    private readonly IForumUseCase _forumUseCase;
    private readonly ForumAggregator.Application.Services.IForumService _forumAppService;
    private readonly IValidator<CreateForumRequest> _createForumRequestValidator;
    private readonly IValidator<AddModeratorRequest> _addModeratorRequestValidator;
    private readonly IMapper _mapper;

    public ForumController(
        IForumUseCase ForumUseCase, 
        IValidator<CreateForumRequest> createForumRequestValidator,
        IMapper mapper,
        ForumAggregator.Application.Services.IForumService forumAppService,
        IValidator<AddModeratorRequest> addModeratorRequestValidator
    )
    {
        _forumUseCase = ForumUseCase;
        _createForumRequestValidator = createForumRequestValidator;
        _mapper = mapper;
        _forumAppService = forumAppService;
        _addModeratorRequestValidator = addModeratorRequestValidator;
    }

    [HttpPost("forum")]
    [Authorize]
    public IActionResult CreateForum(CreateForumRequest createForumRequest)
    {
        var validationResult = _createForumRequestValidator.Validate(createForumRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var result =_forumUseCase.Create(
            createForumRequest.Name,
            createForumRequest.Description,
            _mapper.Map<
                ICollection<Controllers.Forum.Moderator>, 
                ICollection<Application.UseCases.ModeratorUseCaseModel>
            >(createForumRequest.Moderators),
            _mapper.Map<
                ICollection<Controllers.Forum.BlackListed>, 
                ICollection<Application.UseCases.BlackListedUseCaseModel>
            >(createForumRequest.BlackList)
        );

        if (result.Value == false || result.EntityUseCaseDto == null)
        {
            return UnprocessableEntity(result.Result);
        }

        return Ok(new CreateForumResponse(
            result.EntityUseCaseDto.Id, 
            result.EntityUseCaseDto.Name
        ));

        throw new NotImplementedException();
    }

    [HttpPost("forum/moderator")]
    [Authorize]
    public IActionResult AddModerator(AddModeratorRequest addModeratorRequest)
    {
        var validationResult = _addModeratorRequestValidator.Validate(addModeratorRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var moderators =_mapper.Map<
            ICollection<Controllers.Forum.Moderator>, 
            ICollection<Application.UseCases.ModeratorUseCaseModel>
        >(addModeratorRequest.Moderators);

        var result = _forumUseCase.AddModerator(addModeratorRequest.ForumId, moderators);

        if (result.Value == false)
            return UnprocessableEntity(result.Result);

        var moderatorsId = result.Result.Split(",").Select(x => Guid.Parse(x)).ToList();
        
        return Ok(moderatorsId);
    }

    [HttpPatch("forum")]
    [Authorize]
    public IActionResult UpdateForum()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("forum")]
    [Authorize]
    public IActionResult DeleteForum()
    {
        throw new NotImplementedException();
    }

    [HttpGet("forum/{forumId}")]
    [AllowAnonymous]
    public IActionResult ReadForum(string forumId)
    {
        ForumAppServiceModel? forum;
        Guid guidForumId;
        
        if (Guid.TryParse(forumId, out guidForumId) == true)
            forum = _forumAppService.GetForum(guidForumId);
        else
            forum = _forumAppService.GetForumByName(forumId);


        if (forum == null)
            return NotFound($"Forum {forumId} not found");
        
        return Ok(_mapper.Map<ReadForumResponse>(forum));
    }

    [HttpGet("forum")]
    [AllowAnonymous]
    public IActionResult ReadAllForums()
    {
        return Ok(_forumAppService.GetAllForums().Select(x => _mapper.Map<ReadForumResponse>(x)).ToList());
    }
}