using AutoMapper;

namespace BlogAPI
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserCreateDto>().ReverseMap();
            CreateMap<User, UserUpdateDto>().ReverseMap();

            CreateMap<Blog, BlogCreateDto>().ReverseMap();
            CreateMap<Blog, BlogDto>().ReverseMap();
        }
    }
}