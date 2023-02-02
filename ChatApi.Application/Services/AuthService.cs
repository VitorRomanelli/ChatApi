using ChatApi.Application.Models;
using ChatApi.Application.Models.InputModels;
using ChatApi.Application.Services.Interfaces;
using ChatApi.Domain.DTOs;
using ChatApi.Domain.Entities;
using ChatApi.Persistence.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApi.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly SignInManager<User> _signManager;
        private readonly AppDbContext _db;
        public IConfiguration configuration { get; }

        public AuthService(SignInManager<User> signManager, IConfiguration Configuration, AppDbContext db)
        {
            _signManager = signManager;
            configuration = Configuration;
            _db = db;
        }

        public SecurityToken GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration.GetSection("TokenAuthentication")["SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Authentication, user.Id)
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = configuration.GetSection("TokenAuthentication")["Issuer"],
                Audience = configuration.GetSection("TokenAuthentication")["Audience"]
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return token;
        }

        public async Task<ResponseModel> LogIn(AuthLoginModel user)
        {
            var findUser = await _db.Users.FirstOrDefaultAsync(x => x.UserName == user.UserName);
            if (findUser == null)
            {
               return new ResponseModel(401, "Usuario ou Senha incorretos");
            }

            var result = await _signManager.PasswordSignInAsync(findUser, user.Password, false, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return new ResponseModel(401, "Usuario ou Senha incorretos");
            }

            var defaultToken = GenerateToken(findUser);

            return new ResponseModel(200, new AuthenticateUserDTO(
                new JwtSecurityTokenHandler().WriteToken(defaultToken),
                new UserDTO(findUser),
                defaultToken.ValidTo,
                200
            ));
        }

    }
}
