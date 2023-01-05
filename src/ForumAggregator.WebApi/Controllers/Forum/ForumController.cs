namespace ForumAggregator.WebApi.Controllers.Forum;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using System;

[ApiController]
[Route("/")]
public class ForumController: ControllerBase
{
    [HttpPost("forum")]
    [Authorize]
    public IActionResult CreateForum(CreateForumRequest createForumRequest)
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    [HttpGet("forum")]
    [AllowAnonymous]
    public IActionResult ReadAllForums()
    {
        throw new NotImplementedException();
    }
}