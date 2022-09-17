using ChatApi.Entities;
using ChatApi.Helpers;
using ChatApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(string Id)
        {
            return new ResponseHelper().CreateResponse(await _userService.GetByIdAsync(Id));
        }

        [HttpPost]
        [Route("add")]
        [AllowAnonymous]
        public async Task<IActionResult> AddAsync(User user)
        {
            return new ResponseHelper().CreateResponse(await _userService.AddAsync(user));
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditAsync(User user)
        {
            return new ResponseHelper().CreateResponse(await _userService.EditAsync(user));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(string Id)
        {
            return new ResponseHelper().CreateResponse(await _userService.DeleteAsync(Id));
        }
    }
}
