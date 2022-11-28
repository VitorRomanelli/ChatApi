using ChatApi.Domain.Enums;

namespace ChatApi.Domain.Entities
{
    public class ChatGroupMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public eMessageType Type { get; set; }

        public string SenderUserId { get; set; } = string.Empty;
        public User? SenderUser { get; set; }
        
        public Guid GroupId { get; set; }
        public ChatGroup? Group { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
