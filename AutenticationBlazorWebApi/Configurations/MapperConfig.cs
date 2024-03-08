using AutenticationBlazorWebApi.Models;
using AutenticationBlazorWebApi.Models.DTOs;
using AutoMapper;

namespace AutenticationBlazorWebApi.Server.Configurations
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<UserDto, User>().ReverseMap();
        }
    }
}
