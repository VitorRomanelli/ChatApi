using ChatApi.Entities;

namespace ChatApi.Models
{
    public class RoomEnterModel
    {
        public User User { get; set; } = new User();
        public string Message { get; set; } = string.Empty;
        public string RoomId { get; set; } = string.Empty;
    }
}
