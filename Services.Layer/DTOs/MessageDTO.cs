using Data.Layer.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs
{
    public class MessageDTO
    {
        public required int Id { get; set; }
        public required string SenderId { get; set; }
        public required string SenderUsername { get; set; }
        public string? SenderPhotoUrl { get; set; }
        public required string RecipientId { get; set; }
        public required string RecipientUsername { get; set; }
        public string? RecipientPhotoUrl { get; set; }
        public required string Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
