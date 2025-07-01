using AutoMapper;
using Common.Layer;
using Data.Layer.Contexts;
using Data.Layer.Entities;
using Data.Layer.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.Layer.Interfaces;
using Repository.Layer.Specifications.Messages;
using Services.Layer;
using Services.Layer.DTOs;
using Services.Layer.Identity;

namespace DatingAppAPI.SignalR
{
    public class MessageHub : Hub
    {
        private readonly IAccountService _accountService;
        private readonly IMessageService _messageService;
        private UserManager<AppUser> _userManager;
        private IUnitOfWork<AppDbContext> _unitOfWork;
        private IMapper _mapper;
        public MessageHub(IAccountService _accountService, IMapper mapper, IMessageService _messageService, IUnitOfWork<AppDbContext> unitOfWork, UserManager<AppUser> userManager)
        {
            this._accountService = _accountService;
            this._messageService = _messageService;
            this._unitOfWork = unitOfWork;
            this._userManager = userManager;
            this._mapper = mapper;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var CurrentUser = await _accountService.GetCurrentUserUsername();
            var CurrentUserId = _accountService.GetCurrentUserId();
            var OtherUserId = httpContext.Request.Query["user"];

            if (CurrentUser == OtherUserId) throw new HubException("You cannot send messages to yourself");

            var GroupName = GetGroupName(CurrentUserId, OtherUserId);
            await Groups.AddToGroupAsync(Context.ConnectionId, GroupName);

            var specs = new MessageSpecification();
            specs.SenderId = CurrentUserId;
            specs.RecipientId = OtherUserId;

            var messages = await _messageService.GetMessagesThread(specs);

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);
        }

        public async Task SendMessage(CreateMessageDTO message)
        {
            var senderId = _accountService.GetCurrentUserId();
            var recipientId = message.RecipientId;

            var sender = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == senderId);
            var recipient = await _userManager.Users.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Id == recipientId);

            if (sender == null || recipient == null)
            {
                throw new HubException("User not found");
            }

            if (senderId == recipientId)
            {
                throw new HubException("You cannot send messages to yourself");
            }

            message.SenderId = senderId;
            message.RecipientId = recipientId;

            // mapping 
            var messageEntity = _mapper.Map<Message>(message);
            messageEntity.Sender = sender;
            messageEntity.Recipient = recipient;
            messageEntity.RecipientUsername = recipient.UserName!;
            messageEntity.SenderUsername = sender.UserName!;
            // add message
            var createdMessage = await _unitOfWork.Repository<Message, string>().Create(messageEntity);

            var isSaved = await _unitOfWork.CompleteAsync();

            message.Id = messageEntity.Id;
            message.SenderPhotoUrl = sender.Photos.FirstOrDefault(x => x.IsMain)?.Url;
            message.RecipientPhotoUrl = recipient.Photos.FirstOrDefault(x => x.IsMain)?.Url;

            if (isSaved > 0)
            {
                try
                {
                    var groupName = GetGroupName(sender.Id, recipient.Id);
                    await Clients.Group(groupName).SendAsync("NewMessage", message);
                }
                catch (HubException ex)
                {
                    throw new HubException(ex.Message);
                }
            }
            else
            {
                throw new HubException("Failed to send message");
            }
        }

        private string GetGroupName(string? caller, string? otherUser)
        {
            var stringCompare = string.CompareOrdinal(caller, otherUser) < 0;
            return stringCompare ? $"{caller}-{otherUser}" : $"{otherUser}-{caller}";
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }
    }
}
