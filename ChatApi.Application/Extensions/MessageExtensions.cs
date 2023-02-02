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
    public static class MessageExtensions
    {
        public static IQueryable<ChatMessage> ApplyFilter(this IQueryable<ChatMessage> items, Guid chatId)
        {
            items = items.AsNoTracking();
            items = items.Where(x => x.ChatId == chatId);
            return items;
        }
    }
}
