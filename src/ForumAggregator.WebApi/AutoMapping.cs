namespace ForumAggregator.WebApi;

using AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<ForumAggregator.Application.UseCases.ModeratorUseCaseModel, ForumAggregator.WebApi.Controllers.Forum.Moderator>().ReverseMap();
        
        CreateMap<ForumAggregator.Application.UseCases.BlackListedUseCaseModel, ForumAggregator.WebApi.Controllers.Forum.BlackListed>().ReverseMap();
        
        CreateMap<ForumAggregator.Application.Services.ForumAppServiceModel, ForumAggregator.WebApi.Controllers.Forum.ReadForumResponse>().ReverseMap();
    }
}