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
            // Map between Photo and PhotoDTO
            CreateMap<Photo, PhotoDTO>().ReverseMap();

            // Map between Address and AddressDTO
            CreateMap<Address, AddressDTO>().ReverseMap();

            // Map between AppUser and MemberDTO
            CreateMap<AppUser, MemberDTO>()
                .ForMember(dest => dest.PhotoUrl, opt =>
                    opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(dest => dest.Photos, opt => opt.MapFrom(src => src.Photos))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address));

            // Reverse map with condition to ignore null values during updates
            CreateMap<MemberDTO, AppUser>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Handle Address updates specifically to avoid null overwrite
            CreateMap<AddressDTO, Address>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Handle Photo updates specifically to avoid null overwrite
            CreateMap<PhotoDTO, Photo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
