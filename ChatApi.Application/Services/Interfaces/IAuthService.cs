using ChatApi.Application.Models;
using ChatApi.Application.Models.InputModels;
using ChatApi.Domain.Entities;
using Microsoft.IdentityModel.Tokens;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IAuthService
    {
        SecurityToken GenerateToken(User user);
        Task<ResponseModel> LogIn(AuthLoginModel user);
    }
}
