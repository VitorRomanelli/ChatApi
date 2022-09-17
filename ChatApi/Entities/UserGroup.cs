namespace ChatApi.Entities
{
    public class UserGroup
    {
        public Guid Id { get; set; }
        
        public string UserId { get; set; } = string.Empty;
        public User? User { get; set; }

        public Guid GroupId { get; set; }
        public Group? Group { get; set; }
    }
}
