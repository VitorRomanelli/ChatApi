﻿using ChatApi.Domain.Enums;

namespace ChatApi.Domain.Entities
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public string SenderUserId { get; set; } = string.Empty;
        public User? SenderUser { get; set; }

        public eMessageType Type { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public bool Visualized { get; set; }

        public Guid ChatId { get; set; }
        public Chat? Chat { get; set; }
    }
}
