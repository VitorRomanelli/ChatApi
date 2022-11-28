using ChatApi.Domain.Entities;

namespace ChatApi.Application.Models
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

    public class SendMessageSocketModel 
    {
        public ChatMessage? Message { get; set; }
        public Guid? MessageId { get; set; }
        public ChatGroupMessage? GroupMessage { get; set; }
        public Guid? GroupMessageId { get; set; }
    }

    public class RoomSocketModel
    {
        public string? RoomId { get; set; }
        public string? Message { get; set; }
        public string? Type { get; set; }
        public User? User { get; set; }
    }
}
