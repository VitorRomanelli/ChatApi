using ChatApi.Entities;
using ChatApi.Models;

namespace ChatApi.Services.Interfaces
{
    public interface IChatService
    {
        public Task<ResponseModel> SendMessage(SendMessageModel model);
    }
}
