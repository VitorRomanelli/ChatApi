namespace ChatApi.Domain.Entities
{
    public class UserChatGroup
    {
        public Guid Id { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        public Guid GroupId { get; set; }
        public ChatGroup? Group { get; set; }
    }
}
