using ChatApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApi.Domain.DTOs
{
    public class UserDTO
    {
        public UserDTO(User user) 
        { 
            Id = user.Id;
            Name = user.Name;
            Email = user.Email;
            UserName = user.UserName;
            Pic = user.Pic;
            Biography = user.Biography;
        }

        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Pic { get; set; } = string.Empty;
        public string Biography { get; set; } = string.Empty;
    }
}
