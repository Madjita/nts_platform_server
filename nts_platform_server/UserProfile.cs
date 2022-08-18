using System;
using AutoMapper;
using nts_platform_server.Entities;
using nts_platform_server.Models;

namespace nts_platform_server
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<UserModelRegister, User>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                //.ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Username))
                .ForMember(dst => dst.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dst => dst.SecondName, opt => opt.MapFrom(src => src.SecondName))
                .ForMember(dst => dst.MiddleName, opt => opt.MapFrom(src => src.MiddleName))
                .ForMember(dst => dst.Id, opt => opt.Ignore())
                .ForPath(dst => dst.Company.Name, opt=> opt.MapFrom(src => src.Company))
                ;

            CreateMap<User, AuthenticateResponse>()
                .ForMember(dst => dst.Email, opt => opt.MapFrom(src => src.Email))
                //.ForMember(dst => dst.Username, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.FirstName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.SecondName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.MiddleName, opt => opt.MapFrom(src => src.Email))
                .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dst => dst.Token, opt => opt.Ignore())
                ;
        }
    }
}
