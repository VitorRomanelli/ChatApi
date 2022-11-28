using ChatApi.Application.Models;
using ChatApi.Application.Services.Interfaces;
using ChatApi.Domain.Entities;
using ChatApi.Persistence.Data;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _db;

        public GroupService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<ResponseModel> GetByIdAsync(Guid id)
        {
            try
            {
                var group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == id);

                if (group == null)
                {
                    return new ResponseModel(404, "Grupo não encontrado!");
                }

                return new ResponseModel(200, group);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao buscar pelo grupo!");
            }
        }

        public async Task<ResponseModel> GetByUserIdAsync(string userId)
        {
            try
            {
                var group = await _db.Groups.Include(x => x.Users).FirstOrDefaultAsync(g => g.Users.Select(x => x.UserId).Contains(userId));

                if (group == null)
                {
                    return new ResponseModel(404, "Grupo não encontrado!");
                }

                return new ResponseModel(200, group);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao buscar pelo grupo!");
            }
        }

        public async Task<ResponseModel> AddAsync(ChatGroup group)
        {
            try
            {
                await _db.Groups.AddAsync(group);
                await _db.SaveChangesAsync();

                return new ResponseModel(200, "Grupo cadastrado com sucesso");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao cadastrar o grupo!");
            }
        }

        public async Task<ResponseModel> EditAsync(ChatGroup group)
        {
            try
            {
                var findGroup = await _db.Groups.Include(x => x.Users).FirstOrDefaultAsync(g => g.Id == group.Id);

                if (findGroup == null)
                {
                    return new ResponseModel(404, "Grupo não encontrado!");
                }

                findGroup.Name = group.Name;
                findGroup.Description = group.Description;
                findGroup.Pic = group.Pic;
                findGroup.Users = group.Users;
                findGroup.UpdateAt = DateTime.Now;

                _db.Groups.Update(findGroup);
                await _db.SaveChangesAsync();

                return new ResponseModel(200, "Grupo atualizado com sucesso");
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao buscar pelo grupo!");
            }
        }

        public async Task<ResponseModel> DeleteAsync(Guid id)
        {
            try
            {
                var group = await _db.Groups.FirstOrDefaultAsync(g => g.Id == id);

                if (group == null)
                {
                    return new ResponseModel(404, "Grupo não encontrado!");
                }

                _db.Groups.Remove(group);
                await _db.SaveChangesAsync();

                return new ResponseModel(200, group);
            }
            catch
            {
                return new ResponseModel(500, "Ocorreu um erro ao buscar pelo grupo!");
            }
        }
    }
}
