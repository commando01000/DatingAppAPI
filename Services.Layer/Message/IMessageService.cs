using Common.Layer;
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
        Task<MessageDTO> GetMessage(string id);
        Task<PaginatedResultDTO<MessageDTO>> GetMessagesForUser();
        Task<IEnumerable<MessageDTO>> GetMessageThread(string currentUsername, string recipientUsername);
    }
}
