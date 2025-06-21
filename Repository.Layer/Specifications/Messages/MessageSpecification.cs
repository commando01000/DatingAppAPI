using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Layer.Specifications.Messages
{
    public class MessageSpecification
    {
        public int? Id { get; set; }
        public string? SenderId { get; set; }
        public string? SenderUsername { get; set; }
        public string? RecipientId { get; set; }
        public string? RecipientUsername { get; set; }
        public bool? IsRead { get; set; }
        public string? Content { get; set; }
        public string? Container { get; set; } = "Inbox"; // Outbox or Inbox

        private string? _search;
        public string? Search { get => _search; set => _search = value?.ToLower(); }

        public string? UserId { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime? MessageSent { get; set; }
        public bool? SenderDeleted { get; set; }
        public bool? RecipientDeleted { get; set; }

        public int pageSize { get; set; } = 5;
        public int PageIndex { get; set; } = 1;
        private const int MaxPageSize = 50;

        public int PageSize
        {
            get => pageSize;
            set => pageSize = (value > MaxPageSize) ? MaxPageSize : value;
        }
    }
}
