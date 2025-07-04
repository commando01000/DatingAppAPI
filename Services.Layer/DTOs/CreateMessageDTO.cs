﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Layer.DTOs
{
    public class CreateMessageDTO
    {
        public int? Id { get; set; }
        public string? SenderId { get; set; }
        public string? SenderUsername { get; set; }
        public string? SenderPhotoUrl { get; set; }
        public string? RecipientId { get; set; }
        public string? RecipientUsername { get; set; }
        public string? RecipientPhotoUrl { get; set; }
        public string? Content { get; set; }
        public bool IsRead { get; set; }
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.UtcNow;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
    }
}
