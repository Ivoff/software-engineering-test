namespace ForumAggregator.Application.UseCases;

public record UserUseCaseResult(
    bool Value,
    string Result,
    UserUseCaseModel? UseCaseModel
);