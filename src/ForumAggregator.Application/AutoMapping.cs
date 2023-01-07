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
                opts => opts.MapFrom(infraModerator =>  infraModerator.ModeratorAuthorities.Select(auth => (ForumAggregator.Domain.ForumRegistry.EAuthority) auth.Authority))
            );
        
        CreateMap<ForumAggregator.Domain.ForumRegistry.Moderator, ForumAggregator.Infraestructure.Models.Moderator>()
            .ForMember(
                infraModerator => infraModerator.Id,
                opts => opts.MapFrom(domainModerator => domainModerator.Id)
            )
            .ForMember(
                infraModerator => infraModerator.UserId,
                opts => opts.MapFrom(domainModerator => domainModerator.UserId)
            )
            .ForMember(
                infraModerator => infraModerator.Deleted,
                opts => opts.MapFrom(domainModerator => domainModerator.Deleted)
            )
            .ForMember(
                infraModerator => infraModerator.ModeratorAuthorities,
                opts => opts.MapFrom(
                    domainModerator =>  domainModerator.Authorities
                        .Select(auth => new ForumAggregator.Infraestructure.Models.ModeratorAuthority(){
                                ModeratorId = domainModerator.Id,
                                Authority = auth
                            }
                        ).ToList()
                    ));
        
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
            );

        CreateMap<ForumAggregator.Domain.ForumRegistry.Forum, ForumAggregator.Application.Services.ForumAppServiceModel>()
            .ForMember(
                appServiceForum => appServiceForum.Id,
                opts => opts.MapFrom(domainForum => domainForum.Id)
            )
            .ForMember(
                appServiceForum => appServiceForum.OwnerId,
                opts => opts.MapFrom(domainForum => domainForum.OwnerId)
            )
            .ForMember(
                appServiceForum => appServiceForum.Name,
                opts => opts.MapFrom(domainForum => domainForum.Name)
            )
            .ForMember(
                appServiceForum => appServiceForum.Description,
                opts => opts.MapFrom(domainForum => domainForum.Description)
            )
            .ForMember(
                appServiceForum => appServiceForum.Deleted,
                opts => opts.MapFrom(domainForum => domainForum.Deleted)
            )
            .ForMember(
                appServiceForum => appServiceForum.Moderators,
                opts => opts.MapFrom(domainForum => domainForum.ModeratorCollection.Moderators.Select(
                    x => new ForumAggregator.Application.Services.ModeratorAppServiceModel(){
                            Id = x.Id,
                            UserId = x.UserId,
                            Deleted = x.Deleted,
                            Authorities = x.Authorities
                        }
                    )
                )
            )
            .ForMember(
                appServiceForum => appServiceForum.BlackList,
                opts => opts.MapFrom(domainForum => domainForum.BlackListedCollection.BlackList.Select(
                    x => new ForumAggregator.Application.Services.BlackListedAppServiceModel(){
                        Id = x.Id,
                        UserId = x.UserId,
                        CanComment = x.CanComment,
                        CanPost = x.CanPost,
                        Deleted = x.Deleted
                    }
                ))
            ).AfterMap((src, dst) => dst.BlackList.Select(x => {x.ForumId = src.Id; return x;}));
        
        CreateMap<ForumAggregator.Domain.PostRegistry.Post, ForumAggregator.Infraestructure.Models.Post>()
            .ForMember(
                infraPost => infraPost.Id,
                opts => opts.MapFrom(domainPost => domainPost.Id)
            )
            .ForMember(
                infraPost => infraPost.ForumId,
                opts => opts.MapFrom(domainPost => domainPost.ForumId)
            )
            .ForMember(
                infraPost => infraPost.AuthorId,
                opts => opts.MapFrom(domainPost => domainPost.Author.Id)
            )
            .ForMember(
                infraPost => infraPost.Title,
                opts => opts.MapFrom(domainPost => domainPost.Title)
            )
            .ForMember(
                infraPost => infraPost.Content,
                opts => opts.MapFrom(domainPost => domainPost.Content)
            )
            .ForMember(
                infraPost => infraPost.Deleted,
                opts => opts.MapFrom(domainPost => domainPost.Deleted)
            );
    }
}