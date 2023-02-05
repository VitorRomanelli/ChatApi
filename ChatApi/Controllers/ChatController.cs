
using ChatApi.Application.Models;
using ChatApi.Application.Services;
using ChatApi.Application.Services.Interfaces;
using ChatApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize("Bearer")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _service;

        public ChatController(IChatService service)
        {
            _service = service;
        }

        [HttpPost]
        [Route("visualize")]
        [Authorize("Bearer")]
        public async Task<IActionResult> Visualize([FromQuery] Guid chatId, string userId)
        {
            return new ResponseHelper().CreateResponse(await _service.Visualize(userId, chatId));
        }


        [HttpGet]
        [Route("message")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetMessages([FromQuery] Guid chatId, int page)
        {
            return new ResponseHelper().CreateResponse(await _service.GetUserMessages(chatId, page));
        }


        [HttpGet]
        [Route("")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetByUserId()
        {
            string userId = User.FindFirst(ClaimTypes.Authentication)!.Value;
            return new ResponseHelper().CreateResponse(await _service.GetUserChats(userId));
        }

        [HttpPost]
        [Route("paginated")]
        [Authorize("Bearer")]
        public async Task<IActionResult> GetPaginated(ChatFilterModel model)
        {
            string userId = User.FindFirst(ClaimTypes.Authentication)!.Value;
            model.UserId = userId;
            return new ResponseHelper().CreateResponse(await _service.GetUserPaginatedChats(model));
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> Add(AddChatModel model)
        {
            return new ResponseHelper().CreateResponse(await _service.Add(model));
        }

        [HttpPost]
        [Route("message")]
        public async Task<IActionResult> SendMessage(SendMessageModel model)
        {
            return new ResponseHelper().CreateResponse(await _service.SendMessage(model));
        }
    }
}
