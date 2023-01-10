namespace ForumAggregator.Application.UseCases;

using System;

public class BlackListedUseCaseModel
{
    public Guid UserId { get; set; }
    public bool? CanComment { get; set; }
    public bool? CanPost { get; set; }
}