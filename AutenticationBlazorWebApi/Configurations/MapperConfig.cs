using AutenticationBlazorWebApi.Models;
using AutenticationBlazorWebApi.Models.DTOs;
using AutoMapper;

namespace AutenticationBlazorWebApi.Server.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<RegisterDto, User>().ReverseMap();
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<CreateUserDto, User>().ReverseMap();

            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserRole, opt =>
                    opt.MapFrom(src => src.UserRole.Select(ur => new UserRoleDto
                    {
                        UserId = ur.UserId,
                        RoleId = ur.RoleId,
                        Role = new RoleDto
                        {
                            Id = ur.Role.Id,
                            Name = ur.Role.Name
                        }
                    })));

        }
    }
}
