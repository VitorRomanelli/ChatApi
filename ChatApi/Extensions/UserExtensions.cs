using ChatApi.Entities;
using ChatApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ChatApi.Extensions
{
    public static class UserExtensions
    {
        public static IQueryable<User> ApplyFilter(this IQueryable<User> items, UserFilterModel model)
        {
            items = !String.IsNullOrEmpty(model.Name) ? items.Where(x => x.UserName.ToLower().Contains(model.Name)) : items;
            return items;
        }
    }
}
