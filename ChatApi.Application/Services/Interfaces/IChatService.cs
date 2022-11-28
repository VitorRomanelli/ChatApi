using ChatApi.Application.Models;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IChatService
    {
        public Task<ResponseModel> SendMessage(SendMessageModel model);
    }
}
