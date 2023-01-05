namespace ForumAggregator.WebApi.Controllers.Test;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

using ForumAggregator.Domain.Shared.Interfaces;

[ApiController]
[Route("/")]
public class TestController: ControllerBase
{
    private readonly IForumRepository _forumRepository;

    public TestController(IForumRepository forumRepository)
    {
        _forumRepository = forumRepository;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Index()
    {
        _forumRepository.Get(Guid.Parse("360e32f0-21d3-4d34-9bdb-b4f5fbc8bf1c"));
        return Ok();
    }
}