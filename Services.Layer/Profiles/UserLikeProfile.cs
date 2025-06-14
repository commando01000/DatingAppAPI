using AutoMapper;
using Data.Layer.Entities;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.Profiles
{
    public class UserLikeProfile : Profile
    {
        public UserLikeProfile()
        {
            CreateMap<UserLikeDTO, UserLike>()
            .ForMember(dest => dest.SourceUser, opt => opt.Ignore())
            .ForMember(dest => dest.LikedUser, opt => opt.Ignore())
            .ReverseMap();
        }
    }
}
