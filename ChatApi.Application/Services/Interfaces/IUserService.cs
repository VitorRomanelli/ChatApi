using ChatApi.Application.Models;
using ChatApi.Domain.Entities;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseModel> GetPaginated(UserFilterModel model, string userId);
        Task<ResponseModel> GetByIdAsync(string id);
        Task<ResponseModel> AddAsync(User user);
        Task<ResponseModel> EditAsync(User user);
        Task<ResponseModel> DeleteAsync(string id);
    }
}
