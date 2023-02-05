using ChatApi.Application.Models;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IChatService
    {
        public Task<ResponseModel> Visualize(string userId, Guid? chatId);
        public Task<ResponseModel> SendMessage(SendMessageModel model);
        public Task<ResponseModel> Add(AddChatModel model);
        public Task<ResponseModel> GetUserChats(string userId);
        public Task<ResponseModel> GetUserPaginatedChats(ChatFilterModel model);
        public Task<ResponseModel> GetUserMessages(Guid chatId, int page);
    }
}
