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
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            // Message → MessageDTO
            CreateMap<Message, MessageDTO>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src =>
                    src.Sender.Photos.FirstOrDefault(p => p.IsMain) != null
                        ? src.Sender.Photos.First(p => p.IsMain).Url
                        : null))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src =>
                    src.Recipient.Photos.FirstOrDefault(p => p.IsMain) != null
                        ? src.Recipient.Photos.First(p => p.IsMain).Url
                        : null));

            CreateMap<Message, CreateMessageDTO>()
                .ForMember(dest => dest.SenderPhotoUrl, opt => opt.MapFrom(src =>
                    src.Sender.Photos.FirstOrDefault(p => p.IsMain) != null
                        ? src.Sender.Photos.First(p => p.IsMain).Url
                        : null))
                .ForMember(dest => dest.RecipientPhotoUrl, opt => opt.MapFrom(src =>
                    src.Recipient.Photos.FirstOrDefault(p => p.IsMain) != null
                        ? src.Recipient.Photos.First(p => p.IsMain).Url
                        : null));


            // MessageDTO → Message
            CreateMap<MessageDTO, Message>()
                .ForMember(dest => dest.Sender, opt => opt.Ignore())
                .ForMember(dest => dest.Recipient, opt => opt.Ignore());

            CreateMap<CreateMessageDTO, Message>()
                .ForMember(dest => dest.Sender, opt => opt.Ignore())
                .ForMember(dest => dest.Recipient, opt => opt.Ignore());

        }
    }
}
