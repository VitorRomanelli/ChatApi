using ChatApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatApi.Helpers
{
    public class ResponseHelper : ControllerBase
    {
        public IActionResult CreateResponse(ResponseModel response)
        {
            switch (response.Status)
            {
                case 200:
                    return Ok(response);
                case 500:
                    return StatusCode(500, response);
                case 422:
                    return UnprocessableEntity(response);
                case 409:
                    return Conflict(response);
                case 401:
                    return Unauthorized(response);
                case 403:
                    return Forbid(response.Message);
                case 404:
                    return NotFound(response);
                default:
                    return StatusCode(500, response);
            }
        }
    }
}
