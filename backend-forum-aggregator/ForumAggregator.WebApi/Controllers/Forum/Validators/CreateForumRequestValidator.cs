namespace ForumAggregator.WebApi.Controllers.Forum;

using FluentValidation;

public class CreateForumRequestValidator : AbstractValidator<CreateForumRequest>
{
    public CreateForumRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(255);
        
        RuleFor(x => x.Description).MaximumLength(4096)
            .When(x => string.IsNullOrWhiteSpace(x.Description) == false);
        
        RuleForEach(x => x.Moderators).ChildRules(
            moderator => {
                moderator.RuleFor(y => y.UserId).NotEmpty();
                moderator.RuleFor(y => y.Authorities).NotEmpty();
            }
        ).When(x => x.Moderators != null && x.Moderators.Count() > 0);

        RuleForEach(x => x.BlackList).ChildRules(
            blackListed => {
                blackListed.RuleFor(y => y.UserId).NotEmpty();
                
                blackListed.RuleFor(y => y.CanComment)
                    .NotEqual(true)
                    .When(y => y.CanPost == true && y.CanComment != null && y.CanPost != null)
                    .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
                
                blackListed.RuleFor(y => y.CanComment)
                    .NotNull()
                    .When(y => y.CanPost == null)
                    .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
                
                // blackListed.RuleFor(y => y.CanPost)
                //     .NotEqual(true)
                //     .When(y => y.CanPost == true && y.CanPost != null && y.CanComment != null)
                //     .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
                
                // blackListed.RuleFor(y => y.CanPost)
                //     .NotNull()
                //     .When(y => y.CanComment == null)
                //     .WithMessage("Unecessary addition to BlackList when no restriction is imposed.");
            }
        ).When(x => x.BlackList != null && x.BlackList.Count() > 0);
    }
}