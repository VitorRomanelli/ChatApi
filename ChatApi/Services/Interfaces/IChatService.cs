using ChatApi.Entities;
using ChatApi.Models;

namespace ChatApi.Services.Interfaces
{
    public interface IChatService
    {
        public Task<ResponseModel> EnterChat(string roomId, User user);
        public Task<ResponseModel> SendMessage(string roomId, User user, string Message);
    }
}
