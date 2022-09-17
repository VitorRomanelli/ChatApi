using ChatApi.Enums;

namespace ChatApi.Entities
{
    public class GroupMessage
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;

        public eMessageType Type { get; set; }

        public string SenderUserId { get; set; } = string.Empty;
        public User? SenderUser { get; set; }
        
        public Guid GroupId { get; set; }
        public Group? Group { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
