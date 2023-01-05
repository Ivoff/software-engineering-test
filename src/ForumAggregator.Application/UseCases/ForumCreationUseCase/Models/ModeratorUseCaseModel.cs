namespace ForumAggregator.Application.UseCases;

using System;
using System.Collections.Generic;

public class ModeratorUseCaseModel
{
    public Guid UserId { get; set; }
    public ICollection<int> Authorities { get; set; } = default!;
}