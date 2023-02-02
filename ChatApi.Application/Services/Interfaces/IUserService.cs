using ChatApi.Application.Models;
using ChatApi.Application.Models.InputModels;
using ChatApi.Domain.Entities;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel> GetPaginated(UserFilterModel model, string userId);
        Task<ResponseModel> GetByIdAsync(string id);
        Task<ResponseModel> AddAsync(User user);
        Task<ResponseModel> EditAsync(UserInputModel user);
        Task<ResponseModel> DeleteAsync(string id);
    }
}
