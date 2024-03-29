﻿using ChatApi.Domain.DTOs;
using ChatApi.Domain.Entities;

namespace ChatApi.Application.Models
{
    public class AuthenticateUserDTO
    {
        public AuthenticateUserDTO(string Token, UserDTO User, DateTime Expires, int StatusCode)
        {
            this.Token = Token;
            this.User = User;
            this.Expires = Expires;
            this.StatusCode = StatusCode;
        }

        public string Token { get; set; }
        public UserDTO User { get; set; }
        public DateTime Expires { get; set; }
        public int StatusCode { get; set; }
    }
}
