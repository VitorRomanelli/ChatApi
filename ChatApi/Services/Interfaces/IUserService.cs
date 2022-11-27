using ChatApi.Entities;
using ChatApi.Models;

namespace ChatApi.Services.Interfaces
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
