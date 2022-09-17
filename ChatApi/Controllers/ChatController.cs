
using ChatApi.Helpers;
using ChatApi.Models;
using ChatApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        [Route("health")]
        public IActionResult Health()
        {
            return Ok("healthy");
        }

        [HttpGet]
        [Route("enter/{roomId}")]
        public async Task<IActionResult> EnterChat(RoomEnterModel model)
        {
            return new ResponseHelper().CreateResponse(await _service.EnterChat(model.RoomId, model.User));
        }

        [HttpGet]
        [Route("message/{roomId}")]
        public async Task<IActionResult> SendMessage(RoomEnterModel model)
        {
            return new ResponseHelper().CreateResponse(await _service.SendMessage(model.RoomId, model.User, model.Message));
        }
    }
}
