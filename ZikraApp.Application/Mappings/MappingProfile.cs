using AutoMapper;
using ZikraApp.Application.Models;
using ZikraApp.Core.Entities;

namespace ZikraApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Dhikr mappings
            CreateMap<Dhikr, DhikrDto>();
            CreateMap<CreateDhikrDto, Dhikr>();
            CreateMap<UpdateDhikrDto, Dhikr>();

            // UserDhikr mappings
            CreateMap<UserDhikr, UserDhikrDto>();
            CreateMap<CreateUserDhikrDto, UserDhikr>();
            CreateMap<UpdateUserDhikrDto, UserDhikr>();

            // DhikrProgress mappings
            CreateMap<DhikrProgress, DhikrProgressDto>();
            CreateMap<CreateDhikrProgressDto, DhikrProgress>();
        }
    }
} 