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
    public class MessageWithCountSpecification : BaseSpecifications<Message>
    {
        public MessageWithCountSpecification(MessageSpecification specs) : base(msg =>
        (string.IsNullOrEmpty(specs.Search) || msg.Content.ToLower().Contains(specs.Search.ToLower())) &&
        (string.IsNullOrEmpty(specs.SenderId) || msg.SenderId == specs.SenderId) &&
        (string.IsNullOrEmpty(specs.RecipientId) || msg.RecipientId == specs.RecipientId) &&
        (string.IsNullOrEmpty(specs.RecipientUsername) || msg.RecipientUsername == specs.RecipientUsername) &&
        (string.IsNullOrEmpty(specs.SenderUsername) || msg.SenderUsername == specs.SenderUsername) &&
        (!specs.MessageSent.HasValue || msg.MessageSent == specs.MessageSent))

        {

        }
    }
}
