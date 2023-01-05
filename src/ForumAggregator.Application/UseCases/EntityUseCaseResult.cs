namespace ForumAggregator.Application.UseCases;

public record EntityUseCaseResult(
    bool Value,
    string Result,
    EntityUseCaseDto? EntityUseCaseDto
);