namespace ForumAggregator.Application.UseCases;

public interface IForumCreationUseCase
{
    public EntityUseCaseResult Create(
        string name, 
        string description, 
        ICollection<ModeratorUseCaseModel> moderators, 
        ICollection<BlackListedUseCaseModel> blackList
    );
}