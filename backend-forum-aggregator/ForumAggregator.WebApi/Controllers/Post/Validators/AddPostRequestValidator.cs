namespace ForumAggregator.WebApi.Controllers.Post;

using FluentValidation;

public class AddPostRequestValidator : AbstractValidator<AddPostRequest>
{
    public AddPostRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(1024);
        RuleFor(x => x.Content).NotEmpty().MinimumLength(32);
        RuleFor(x => x.ForumId).NotEmpty();
    }
}