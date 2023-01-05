namespace ForumAggregator.Application;

using AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<ForumAggregator.Infraestructure.Models.User, ForumAggregator.Domain.UserRegistry.User>()
            .ForMember(
                domainUser => domainUser.Id,
                opts => opts.MapFrom(infraUser => infraUser.Id)
            )
            .ForMember(
                domainUser => domainUser.Name,
                opts => opts.MapFrom(infraUser => infraUser.Name)
            )
            .ForMember(
                domainUser => domainUser.Password,
                opts => opts.MapFrom(infraUser => infraUser.Password)
            )
            .ForMember(
                domainUser => domainUser.Email,
                opts => opts.MapFrom(infraUser => infraUser.Email)
            )
            .ForMember(
                domainUser => domainUser.Deleted,
                opts => opts.MapFrom(infraUser => infraUser.Deleted)
            ).ReverseMap();
        
        CreateMap<
            ForumAggregator.Domain.UserRegistry.User, 
            ForumAggregator.Application.Services.UserAppServiceModel
        >().ReverseMap();

        CreateMap<ForumAggregator.Infraestructure.Models.Forum, ForumAggregator.Domain.ForumRegistry.Forum>()
            .ForMember(
                domainForum => domainForum.Id,
                opts => opts.MapFrom(infraForum => infraForum.Id)
            )
            .ForMember(
                domainForum => domainForum.OwnerId,
                opts => opts.MapFrom(infraForum => infraForum.OwnerId)
            )
            .ForMember(
                domainForum => domainForum.Name,
                opts => opts.MapFrom(infraForum => infraForum.Name)
            )
            .ForMember(
                domainForum => domainForum.Description,
                opts => opts.MapFrom(infraForum => infraForum.Description)
            )
            .ForMember(
                domainForum => domainForum.Deleted,
                opts => opts.MapFrom(infraForum => infraForum.Deleted)
            ).ReverseMap();

        CreateMap<ForumAggregator.Infraestructure.Models.Moderator, ForumAggregator.Domain.ForumRegistry.Moderator>()
            .ForMember(
                domainModerator => domainModerator.Id,
                opts => opts.MapFrom(infraModerator => infraModerator.Id)
            )
            .ForMember(
                domainModerator => domainModerator.UserId,
                opts => opts.MapFrom(infraModerator => infraModerator.UserId)
            )
            .ForMember(
                domainModerator => domainModerator.Deleted,
                opts => opts.MapFrom(infraModerator => infraModerator.Deleted)
            )
            .ForMember(
                domainModerator => domainModerator.Authorities,
                opts => opts.MapFrom(infraModerator =>  infraModerator.ModeratorAuthorities.Select(auth => auth.Authority))
            ).ReverseMap();
        
        CreateMap<ForumAggregator.Infraestructure.Models.BlackListed, ForumAggregator.Domain.ForumRegistry.BlackListed>().ReverseMap();

        CreateMap<ForumAggregator.Application.Services.ForumAppServiceModel, ForumAggregator.Domain.ForumRegistry.Forum>()
            .ForMember(
                domainForum => domainForum.Id,
                opts => opts.MapFrom(appServiceForum => appServiceForum.Id)
            )
            .ForMember(
                domainForum => domainForum.OwnerId,
                opts => opts.MapFrom(appServiceForum => appServiceForum.OwnerId)
            )
            .ForMember(
                domainForum => domainForum.Name,
                opts => opts.MapFrom(appServiceForum => appServiceForum.Name)
            )
            .ForMember(
                domainForum => domainForum.Description,
                opts => opts.MapFrom(appServiceForum => appServiceForum.Description)
            )
            .ForMember(
                domainForum => domainForum.Deleted,
                opts => opts.MapFrom(appServiceForum => appServiceForum.Deleted)
            ).ReverseMap();

    }
}