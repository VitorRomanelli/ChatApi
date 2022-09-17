using ChatApi.Entities;
using ChatApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace ChatApi.Services.Interfaces
{
    public interface IAuthService
    {
        SecurityToken GenerateToken(User user);
        Task<ResponseModel> LogIn(User user);
    }
}
