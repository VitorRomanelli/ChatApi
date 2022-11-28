namespace ChatApi.Application.Models
{
    public class UserFilterModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
