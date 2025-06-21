using Data.Layer.Entities;
using Repository.Layer.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specifications.Messages
{
    public class MessageWithSpecifications : BaseSpecifications<Message>
    {
        public MessageWithSpecifications(MessageSpecification specs) : base(msg =>
        (string.IsNullOrEmpty(specs.Search) || msg.Content.ToLower().Contains(specs.Search.ToLower())) &&
        (!specs.MessageSent.HasValue || msg.MessageSent == specs.MessageSent) &&
        (!specs.IsRead.HasValue || msg.IsRead == specs.IsRead) &&
        (specs.Container == "Inbox" ? msg.RecipientId == specs.UserId :
         specs.Container == "Outbox" ? msg.SenderId == specs.UserId : specs.Container == "Unread" ? msg.RecipientId == specs.UserId && !msg.IsRead : false)

        )
        {
            // include sender and recipient main photo
            if (specs.UserId != null)
            {
                AddInclude(msg => msg.Sender.Photos.Where(p => p.IsMain));
                AddInclude(msg => msg.Recipient.Photos.Where(p => p.IsMain));
            }

            if (specs.MessageSent.HasValue)
            {
                AddOrderByAsc(msg => msg.MessageSent);
            }

            if (specs.IsRead.HasValue)
            {
                AddOrderByAsc(msg => msg.IsRead);
            }

            ApplyPaging(specs.PageSize * (specs.PageIndex - 1), specs.PageSize);
        }

        public MessageWithSpecifications(int id) : base(msg => msg.Id == id)
        {
            AddInclude(msg => msg.Sender.Photos.Where(p => p.IsMain));
            AddInclude(msg => msg.Recipient.Photos.Where(p => p.IsMain));
        }
    }
}
