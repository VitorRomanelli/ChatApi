﻿using ChatApi.Entities;
using ChatApi.Helpers;
using ChatApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(User user)
        {
            var resp = new ResponseHelper().CreateResponse(await _authService.LogIn(user));
            return resp;
        }
    }
}
