namespace ForumAggregator.WebApi.Controllers.Forum;

using FluentValidation;

public class UpdateForumRequestValidator : AbstractValidator<UpdateForumRequest>
{
    public UpdateForumRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        RuleFor(x => x.Description).MaximumLength(4096)
            .When(x => string.IsNullOrWhiteSpace(x.Description) == false);
    }
}