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
    }
}