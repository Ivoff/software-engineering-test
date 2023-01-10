namespace ForumAggregator.WebApi.Controllers.Post;

using FluentValidation;

public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
{
    public UpdatePostRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty()
            .MaximumLength(1024)
            .When(x => string.IsNullOrWhiteSpace(x.Title) == false);
        
        RuleFor(x => x.Content)
            .NotEmpty()
            .MinimumLength(32)
            .When(x => string.IsNullOrWhiteSpace(x.Content) == false);
        
        RuleFor(x => x.Title)
            .NotEmpty()
            .When(x => string.IsNullOrWhiteSpace(x.Content));
    }
}