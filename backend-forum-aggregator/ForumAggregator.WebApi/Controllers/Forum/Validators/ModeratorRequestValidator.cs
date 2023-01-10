namespace ForumAggregator.WebApi.Controllers.Forum;

using FluentValidation;

public class ModeratorRequestValidator : AbstractValidator<ModeratorRequest>
{
    public ModeratorRequestValidator()
    {
        RuleSet("Default", () => {
            RuleFor(x => x.ForumId).NotEmpty();
            RuleFor(x => x.Moderators).NotEmpty();
            RuleForEach(x => x.Moderators).ChildRules(
                moderator => {
                    moderator.RuleFor(y => y.UserId).NotEmpty();
                    moderator.RuleFor(y => y.Authorities).NotEmpty();
                }
            );
        });

        RuleSet("Delete", () => {
            RuleFor(x => x.ForumId).NotEmpty();
            RuleFor(x => x.Moderators).NotEmpty();
            RuleForEach(x => x.Moderators).ChildRules(
                moderator => {
                    moderator.RuleFor(y => y.UserId).NotEmpty();
                    moderator.RuleFor(y => y.Authorities).Empty();
                }
            );
        });
    }
}