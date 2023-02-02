using ChatApi.Domain.Enums;

namespace ChatApi.Application.Models
{
    public class AddChatModel
    {
        public string SenderUserId { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;
    }

    public class ChatFilterModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Name { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }

    public class SendMessageModel
    {
        public Guid ChatId { get; set; }

        public bool IsToGroup { get; set; }
        public Guid? GroupId { get; set; }

        public string SenderUserId { get; set; } = string.Empty;
        public string RecipientUserId { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;
        public eMessageType Type { get; set; }
    }
}
