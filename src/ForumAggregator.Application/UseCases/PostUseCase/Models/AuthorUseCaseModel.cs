namespace ForumAggregator.Application.UseCases;

using System;

public class AuthorUseCaseModel
{
    public Guid AuthorId { get; set; }
    public bool CannotPost { get; set; }
}