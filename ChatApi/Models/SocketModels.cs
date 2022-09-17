using ChatApi.Entities;

namespace ChatApi.Models
{
    public class SocketMessage
    {
        public string ReceiveId { get; set; }
        public string Message { get; set; }

        public SocketMessage(string message, string receiveId)
        {
            Message = message;
            ReceiveId = receiveId;
        }
    }

    public class RoomSocketModel
    {
        public string? RoomId { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public User? User { get; set; }
    }
}
