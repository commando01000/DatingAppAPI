using Common.Layer;
using Repository.Layer.Specifications.Messages;
using Services.Layer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer
{
    public interface IMessageService
    {
        Task<Response<MessageDTO>> AddMessage(MessageDTO message);
        void DeleteMessage(MessageDTO message);
        Task<MessageDTO> GetMessage(int id);
        Task<PaginatedResultDTO<MessageDTO>> GetMessagesForUser(MessageSpecification messageSpecification);
        //Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);
    }
}
