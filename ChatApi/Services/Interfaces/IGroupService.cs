using ChatApi.Entities;
using ChatApi.Models;

namespace ChatApi.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ResponseModel> GetByIdAsync(Guid id);
        Task<ResponseModel> GetByUserIdAsync(string userId);
        Task<ResponseModel> AddAsync(Group group);
        Task<ResponseModel> EditAsync(Group group);
        Task<ResponseModel> DeleteAsync(Guid id);
    }
}
