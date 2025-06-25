using AutoMapper;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Repository.Layer.Interfaces;
using Repository.Layer.Specifications.Messages;
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

        public async Task<Response<Nothing>> DeleteMessage(int Id)
        {
            var message = await _unitOfWork.Repository<Message, int>().Get(Id);
            var user = _accountService.GetCurrentUserId();

            if (message.SenderId != user && message.RecipientId != user)
            {
                return new Response<Nothing>()
                {
                    Data = null,
                    Message = "You cannot delete this message",
                    Status = false,
                    StatusCode = 400
                };
            }

            if (message != null)
            {
                message.SenderDeleted = true;
                message.RecipientDeleted = true;
                var res = await _unitOfWork.Repository<Message, int>().Delete(message);
                var isSaved = await _unitOfWork.CompleteAsync();
                return new Response<Nothing>()
                {
                    Data = null,
                    Message = "Message deleted successfully",
                    Status = true,
                    StatusCode = 200
                };
            }
            else
            {
                return new Response<Nothing>()
                {
                    Data = null,
                    Message = "Message not found",
                    Status = false,
                    StatusCode = 404
                };
            }
        }

        public Task<MessageDTO> GetMessage(int id)
        {
            var MessageWithSpecs = new MessageWithSpecifications(id);

            var message = _unitOfWork.Repository<Message, string>().GetWithSpecs(MessageWithSpecs);

            return _mapper.Map<Task<MessageDTO>>(message);
        }

        public async Task<PaginatedResultDTO<MessageDTO>> GetMessagesForUser(MessageSpecification messageParams)
        {
            var MessagesWithSpecs = new MessageWithSpecifications(messageParams);
            var MessagesWithCountSpecs = new MessageWithCountSpecification(messageParams);

            var MessagesCount = await _unitOfWork.Repository<Message, string>().GetCountAsync(MessagesWithSpecs);

            var messages = await _unitOfWork.Repository<Message, string>().GetAllWithSpecs(MessagesWithSpecs);

            var MappedMessages = _mapper.Map<IEnumerable<MessageDTO>>(messages); // Mapped

            var PaginatedResult = new PaginatedResultDTO<MessageDTO>(MessagesCount, messageParams.PageIndex, messageParams.PageSize, MappedMessages);

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    if (!message.IsRead)
                    {
                        try
                        {
                            message.IsRead = true;
                            message.DateRead = DateTime.UtcNow;
                            var res = await _unitOfWork.Repository<Message, string>().Update(message);
                            var isSaved = await _unitOfWork.CompleteAsync();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message);
                        }
                    }
                }
            }

            return PaginatedResult;
        }
    }
}
