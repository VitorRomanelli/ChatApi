namespace ChatApi.Domain.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public string SenderUserId { get; set; } = string.Empty;
        public User? SenderUser { get; set; }

        public string RecipientUserId { get; set; } = string.Empty;
        public User? RecipientUser { get; set; }

        public List<ChatMessage>? Messages { get; set; }
    }
}
