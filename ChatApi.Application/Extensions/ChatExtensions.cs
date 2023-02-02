using ChatApi.Application.Models;
using ChatApi.Domain.DTOs;
using ChatApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApi.Application.Extensions
{
    public static class ChatExtensions
    {
        public static IQueryable<Chat> ApplyFilter(this IQueryable<Chat> items, ChatFilterModel model)
        {
            items = items.AsNoTracking().Include(x => x.RecipientUser).Include(x => x.SenderUser);
            items = !String.IsNullOrEmpty(model.Name) ? items.Where(x => x.RecipientUser!.Name.ToLower().Contains(model.Name) || x.SenderUser!.Name.ToLower().Contains(model.Name)) : items;
            items = !String.IsNullOrEmpty(model.UserId) ? items.Where(x => x.SenderUserId == model.UserId || x.RecipientUserId == model.UserId) : items;
            return items;
        }

        public static IQueryable<ChatReducedDTO> MapToReducedDTO(this IQueryable<Chat> items, string userId)
        {
            return items.Include(x => x.SenderUser).Include(x => x.RecipientUser).Select(x => new ChatReducedDTO(x.Id, x.RecipientUserId == userId ? x.SenderUser! : x.RecipientUser!));
        }
    }
}
