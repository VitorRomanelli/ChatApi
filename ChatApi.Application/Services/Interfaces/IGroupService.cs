using ChatApi.Application.Models;
using ChatApi.Domain.Entities;

namespace ChatApi.Application.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ResponseModel> GetByIdAsync(Guid id);
        Task<ResponseModel> GetByUserIdAsync(string userId);
        Task<ResponseModel> AddAsync(ChatGroup group);
        Task<ResponseModel> EditAsync(ChatGroup group);
        Task<ResponseModel> DeleteAsync(Guid id);
    }
}
