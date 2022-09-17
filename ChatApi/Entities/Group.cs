namespace ChatApi.Entities
{
    public class Group
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;

        public List<UserGroup> Users { get; set; }
    }
}
