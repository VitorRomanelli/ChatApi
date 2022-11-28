namespace ChatApi.Domain.Entities
{
    public class Chat
    {
        public Guid Id { get; set; }

        public List<ChatMessage>? Messages { get; set; }
    }
}
