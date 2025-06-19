using AutoMapper;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Repository.Layer.Interfaces;
using Services.Layer.DTOs;
using Services.Layer.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer
{
    public class MessageService : IMessageService
    {
        private readonly IAccountService _accountService;
        private UserManager<AppUser> _userManager;
        private IUnitOfWork<AppDbContext> _unitOfWork;
        private IMapper _mapper;
        public MessageService(IAccountService accountService, UserManager<AppUser> userManager, IMapper mapper, IUnitOfWork<AppDbContext> unitOfWork)
        {
            _accountService = accountService;
            _userManager = userManager;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<MessageDTO>> AddMessage(MessageDTO message)
        {
            var senderId = _accountService.GetCurrentUserId();
            var recipientId = message.RecipientId;

            var sender = await _userManager.FindByIdAsync(senderId);
            var recipient = await _userManager.FindByIdAsync(recipientId);

            if (sender == null || recipient == null)
            {
                return new Response<MessageDTO>
                {
                    Message = "Cannot send message at this time",
                    Status = false,
                    StatusCode = 404
                };
            }

            if (senderId == recipientId)
            {
                return new Response<MessageDTO>
                {
                    Message = "You cannot send messages to yourself",
                    Status = false,
                    StatusCode = 400
                };
            }

            // mapping 
            var messageEntity = _mapper.Map<Message>(message);
            messageEntity.Sender = sender;
            messageEntity.Recipient = recipient;

            // add message
            var createdMessage = await _unitOfWork.Repository<Message, string>().Create(messageEntity);

            var isSaved = await _unitOfWork.CompleteAsync();

            if (isSaved > 0)
            {
                return new Response<MessageDTO>
                {
                    Message = "Message sent successfully",
                    Status = true,
                    StatusCode = 200,
                    Data = message
                };
            }
            else
            {
                return new Response<MessageDTO>
                {
                    Message = "Cannot send message at this time",
                    Status = false,
                    StatusCode = 404,
                    Data = null
                };
            }
        }

        public void DeleteMessage(MessageDTO message)
        {
            throw new NotImplementedException();
        }

        public Task<MessageDTO> GetMessage(string id)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResultDTO<MessageDTO>> GetMessagesForUser()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername)
        {
            throw new NotImplementedException();
        }
    }
}
