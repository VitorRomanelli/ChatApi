using ChatApi.Entities;
using ChatApi.Helpers;
using ChatApi.Models;
using ChatApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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

        [HttpPost]
        [Route("paginated")]
        public async Task<IActionResult> GetPaginated(UserFilterModel model)
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.Name)?.Value;
            return new ResponseHelper().CreateResponse(await _userService.GetPaginated(model, userId));
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
