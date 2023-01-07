namespace ForumAggregator.WebApi.Controllers.Post;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

using ForumAggregator.Domain.Shared.Interfaces;

[ApiController]
[Route("/")]
public class PostController: ControllerBase
{

    public PostController()
    {
    }

    [HttpPost("post")]
    [AllowAnonymous]
    public IActionResult AddPost(PostRequest addPostRequest)
    {
        throw new NotImplementedException();
    }
}