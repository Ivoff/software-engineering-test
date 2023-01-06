namespace ForumAggregator.WebApi.Controllers.Forum;

using FluentValidation;

public class AddModeratorRequestValidator : AbstractValidator<AddModeratorRequest>
{
    public AddModeratorRequestValidator()
    {
        RuleFor(x => x.ForumId).NotEmpty();
        RuleFor(x => x.Moderators).NotEmpty();
        RuleForEach(x => x.Moderators).ChildRules(
            moderator => {
                moderator.RuleFor(y => y.UserId).NotEmpty();
                moderator.RuleFor(y => y.Authorities).NotEmpty();
            }
        );
    }
}