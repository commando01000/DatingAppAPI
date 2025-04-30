using AutoMapper;
using Data.Layer.Entities.Identity;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<Photo, PhotoDTO>().ReverseMap();
            // Map between Address and AddressDTO (optional but recommended)
            CreateMap<Address, AddressDTO>().ReverseMap();

            // Map between AppUser and UserDTO
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address)).ReverseMap();
        }
    }
}
