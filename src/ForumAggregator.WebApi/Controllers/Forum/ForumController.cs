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
    private readonly IValidator<ModeratorRequest> _moderatorRequestValidator;
    private readonly IValidator<UpdateForumRequest> _updateForumRequestValidator;
    private readonly IMapper _mapper;

    public ForumController(
        IForumUseCase ForumUseCase, 
        IValidator<CreateForumRequest> createForumRequestValidator,
        IMapper mapper,
        ForumAggregator.Application.Services.IForumService forumAppService,
        IValidator<ModeratorRequest> addModeratorRequestValidator,
        IValidator<UpdateForumRequest> updateForumRequestValidator
    )
    {
        _forumUseCase = ForumUseCase;
        _createForumRequestValidator = createForumRequestValidator;
        _mapper = mapper;
        _forumAppService = forumAppService;
        _moderatorRequestValidator = addModeratorRequestValidator;
        _updateForumRequestValidator = updateForumRequestValidator;
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
    public IActionResult AddModerator(ModeratorRequest ModeratorRequest)
    {
        var validationResult = _moderatorRequestValidator.Validate(ModeratorRequest, (options) => {
            options.IncludeRuleSets("Default");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var moderators =_mapper.Map<
            ICollection<Controllers.Forum.Moderator>, 
            ICollection<Application.UseCases.ModeratorUseCaseModel>
        >(ModeratorRequest.Moderators);

        var result = _forumUseCase.AddModerator(ModeratorRequest.ForumId, moderators);

        if (result.Value == false)
            return UnprocessableEntity(result.Result);

        var moderatorsId = result.Result.Split(",").Select(x => Guid.Parse(x)).ToList();
        
        return Ok(moderatorsId);
    }

    [HttpPatch("forum")]
    [Authorize]
    public IActionResult UpdateForum(UpdateForumRequest updateRequest)
    {
        var validationResult = _updateForumRequestValidator.Validate(updateRequest);
        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }
        
        var result = _forumAppService.UpdateForum(updateRequest.forumId, updateRequest.Name, updateRequest.Description);

        if (result.Value == false)
            return UnprocessableEntity(result.Result);
        
        return Ok();
    }

    [HttpPatch("forum/moderator")]
    [Authorize]
    public IActionResult UpdateModeratorAuthorities(ModeratorRequest updateModeratorRequest)
    {
        var validationResult = _moderatorRequestValidator.Validate(updateModeratorRequest, (options) => {
            options.IncludeRuleSets("Default");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var result = _forumUseCase.UpdateModeratorAuthorities(
            updateModeratorRequest.ForumId,
            _mapper.Map<ICollection<Moderator>, ICollection<ModeratorUseCaseModel>>(updateModeratorRequest.Moderators)
        );

        if (result.Value == false)
        {
            return UnprocessableEntity(result.Result);
        }

        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
    }

    [HttpDelete("forum")]
    [Authorize]
    public IActionResult DeleteForum()
    {
        throw new NotImplementedException();
    }

    [HttpDelete("forum/moderator")]
    [Authorize]
    public IActionResult DeleteModerator(ModeratorRequest deleteModeratorRequest)
    {
        var validationResult = _moderatorRequestValidator.Validate(deleteModeratorRequest, (options) => {
            options.IncludeRuleSets("Delete");
        });

        if (validationResult.IsValid == false)
        {
            return BadRequest(validationResult.ToString());
        }

        var result = _forumUseCase.RemoveModerator(
            deleteModeratorRequest.ForumId, 
            _mapper.Map<ICollection<Moderator>, ICollection<ModeratorUseCaseModel>>(deleteModeratorRequest.Moderators)
        );
        
        if (result.Value == false)
        {
            return UnprocessableEntity(result.Result);
        }

        return Ok(result.Result.Split(",").Select(x => Guid.Parse(x)).ToList());
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