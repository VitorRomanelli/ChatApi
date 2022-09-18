using ChatApi.Entities;
using ChatApi.Enums;

namespace ChatApi.Models
{
    public class SendMessageModel
    {
        public bool IsToGroup { get; set; }
        public Guid? GroupId { get; set; }

        public string SenderUserId { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
        public eMessageType Type { get; set; }
    }
}
