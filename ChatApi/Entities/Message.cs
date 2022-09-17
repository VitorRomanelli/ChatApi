using ChatApi.Enums;

namespace ChatApi.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public eMessageType Type { get; set; }
        
        public string SenderUserId { get; set; } = string.Empty;
        public User? SenderUser { get; set; }

        public string RecipientUserId { get; set; } = string.Empty;
        public User? RecipientUser { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
