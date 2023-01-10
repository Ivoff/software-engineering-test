namespace ForumAggregator.WebApi.Controllers.Comment;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;

[ApiController]
[Route("/")]
public class CommentController: ControllerBase
{
    public CommentController()
    {

    }

    [HttpPost("comment")]
    [AllowAnonymous]
    public IActionResult AddComment(AddCommentRequest addCommentRequest)
    {
        
        throw new NotImplementedException();
    }
}