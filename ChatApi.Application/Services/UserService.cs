using ChatApi.Application.Extensions;
using ChatApi.Application.Helpers;
using ChatApi.Application.Models;
using ChatApi.Application.Models.InputModels;
using ChatApi.Application.Services.Interfaces;
using ChatApi.Domain.DTOs;
using ChatApi.Domain.Entities;
using ChatApi.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _db;
        private readonly UserManager<User> _userManager;

        public UserService(AppDbContext db, UserManager<User> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public async Task<ResponseModel> GetPaginated(UserFilterModel model, string userId)
        {
            try
            {
                IQueryable<User> users = _db.Users.Where(x => x.Id != userId);
                var result = await users.ApplyFilter(model).MapToDTO().ReturnPaginated(model.Page);

                return new ResponseModel(200, result);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> GetByIdAsync(string id)
        {
            try
            {
                var findUser =  await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (findUser == null)
                {
                    return new ResponseModel(404, "Usuário não encontrado");
                }

                return new ResponseModel(200, findUser);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> AddAsync(User user)
        {
            try
            {
                var findUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);

                if (findUser != null)
                {
                    return new ResponseModel(409, "Nome de usuário em uso");
                } 

                var result = await _userManager.CreateAsync(user, user.Password);

                if(!result.Succeeded)
                {
                    return new ResponseModel(500, "Ocorreu um erro ao cadastrar o usuário!");
                }

                return new ResponseModel(200, "Usuário cadastrado com sucesso");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> EditAsync(UserInputModel user)
        {
            try
            {
                var findUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == user.Id);

                if (findUser == null)
                {
                    return new ResponseModel(404, "Usuário não encontrado");
                }

                findUser.Name = user.Name;
                findUser.Email = user.Email;
                findUser.Biography = user.Biography;

                if (!String.IsNullOrEmpty(user.Pic) && !user.Pic.Contains("Upload"))
                {
                    findUser.Pic = MediaHelper.SaveImage(user.Pic, $"Users/{findUser.Id}", user.PicExtension!, $"pic.{user.PicExtension}");
                }

                var result = await _userManager.UpdateAsync(findUser);

                if (!result.Succeeded)
                {
                    return new ResponseModel(500, "Ocorreu um erro ao atualizar o usuário!");
                }

                return new ResponseModel(200, new UserDTO(findUser));
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }

        public async Task<ResponseModel> DeleteAsync(string id)
        {
            try
            {
                var findUser = await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

                if (findUser == null)
                {
                    return new ResponseModel(404, "Usuário não encontrado");
                }
                
                var result = await _userManager.DeleteAsync(findUser);

                if (!result.Succeeded)
                {
                    return new ResponseModel(500, "Ocorreu um erro ao atualizar o usuário!");
                };

                return new ResponseModel(200, "Usuário removido com sucesso");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro!");
            }
        }
    }
}
