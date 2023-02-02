using ChatApi.Application.Models;
using ChatApi.Domain.DTOs;
using ChatApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Application.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> ApplyFilter(this IQueryable<User> items, UserFilterModel model)
        {
            items = items.AsNoTracking();
            items = !String.IsNullOrEmpty(model.Name) ? items.Where(x => x.UserName.ToLower().Contains(model.Name)) : items;
            return items;
        }

        public static IQueryable<UserDTO> MapToDTO(this IQueryable<User> items)
        {
            return items.Select(x => new UserDTO(x));
        }
    }
}
