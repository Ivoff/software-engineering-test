namespace ForumAggregator.WebApi.Controllers.Forum;

using FluentValidation;

public class BlackListedRequestValidator : AbstractValidator<BlackListedRequest>
{
    public BlackListedRequestValidator()
    {
        RuleFor(x => x.forumId).NotEmpty();
        RuleForEach(x => x.blackListedUsers).ChildRules(
            x => {
                x.RuleFor(x => x.UserId).NotEmpty();
                
                x.RuleFor(y => y.CanComment)
                    .NotEqual(true)
                    .When(y => y.CanPost == true && y.CanComment != null && y.CanPost != null)
                    .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
                
                x.RuleFor(y => y.CanComment)
                    .NotNull()
                    .When(y => y.CanPost == null)
                    .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
                
                // x.RuleFor(y => y.CanPost)
                //     .NotEqual(true)
                //     .When(y => y.CanPost == true && y.CanPost != null && y.CanComment != null)
                //     .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
                
                // x.RuleFor(y => y.CanPost)
                //     .NotNull()
                //     .When(y => y.CanComment == null)
                //     .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
            }
        );
    }
}