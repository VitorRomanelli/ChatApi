using ChatApi.Application.Models;
using ChatApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IAuthService
    {
        SecurityToken GenerateToken(User user);
        Task<ResponseModel> LogIn(User user);
    }
}
