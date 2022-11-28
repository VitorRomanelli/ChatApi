using ChatApi.Application.Services.Interfaces;
using ChatApi.Domain.Entities;
using ChatApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class GroupController : ControllerBase
    {
        private readonly IGroupService _groupService;

        public GroupController(IGroupService groupService)
        {
            _groupService = groupService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetByIdAsync(Guid Id)
        {
            return new ResponseHelper().CreateResponse(await _groupService.GetByIdAsync(Id));
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetByUserIdAsync(string userId)
        {
            return new ResponseHelper().CreateResponse(await _groupService.GetByUserIdAsync(userId));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> AddAsync(ChatGroup group)
        {
            return new ResponseHelper().CreateResponse(await _groupService.AddAsync(group));
        }

        [HttpPost]
        [Route("edit")]
        public async Task<IActionResult> EditAsync(ChatGroup group)
        {
            return new ResponseHelper().CreateResponse(await _groupService.EditAsync(group));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteAsync(Guid Id)
        {
            return new ResponseHelper().CreateResponse(await _groupService.DeleteAsync(Id));
        }
    }
}
