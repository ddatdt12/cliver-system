using AutoMapper;
using CliverSystem.DTOs;
using CliverSystem.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);
        
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<CreatePostDto, Post>()
            .ForMember(p => p.Tags, options => options.MapFrom(p => string.Join(';', p.Tags).Replace(" ","")));

        CreateMap<Package, PackageDto>();
        CreateMap<PackageDto, Package>();
        CreateMap<UpsertPackageDto, Package>();
        //CreateMap<IEnumerable<PackageDto>, IEnumerable<Package>>();

        CreateMap<UpdatePostDto, Post>()
            .ForMember(d => d.Packages, o => o.Ignore())
            .ForMember(d => d.Tags, options => options.MapFrom(p => p.Tags != null ? string.Join(';', p.Tags).Replace(" ", "") : null))
            .ForMember(d => d.Images, options => options.MapFrom(p => p.Images != null ? string.Join(';', p.Images).Replace(" ", "") : null))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>  srcMember != null ));

        CreateMap<Post, PostDto>()
            .ForMember(pDto => pDto.Tags, options => options.MapFrom(p => p.Tags.Split(';', StringSplitOptions.None)))
            .ForMember(pDto => pDto.Images, options => options.MapFrom(p => p.Images.Split(';', StringSplitOptions.None)));


        //Order
        CreateMap<Order, OrderDto>();
        CreateMap<OrderDto, Order>();
        CreateMap<CreateOrderDto, Order>();
    }
}
