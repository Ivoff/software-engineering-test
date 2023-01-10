namespace ForumAggregator.WebApi.Controllers.User;

using FluentValidation;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleSet("Name", () => {
            RuleFor(x => x.NewName).NotEmpty().MaximumLength(255);
        });
        RuleSet("Password", () => {
            RuleFor(x => x.NewPassword).NotEmpty().MinimumLength(8).MaximumLength(255);
        });
        RuleSet("Email", () => {
            RuleFor(x => x.NewEmail).NotEmpty().EmailAddress();
        });
    }
}