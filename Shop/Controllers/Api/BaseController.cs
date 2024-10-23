using Microsoft.AspNetCore.Mvc;

namespace Shop.Controllers.Api
{
    public class ApiResponse
    {
        public bool Success { get; set; }
        public object? Data { get; set; }
        public string? Message { get; set; }
    }

    public class BaseController : Controller
    {

        public IActionResult SendResponse(object result, string message)
        {
            var response = new ApiResponse
            {
                Success = true,
                Data = result,
                Message = message
            };
            return Ok(response);
        }

        public IActionResult SendError(string error, object? errorMessages = null, int code = 404)
        {
            var response = new ApiResponse
            {
                Success = false,
                Message = error,
                Data = errorMessages
            };
            return StatusCode(code, response);
        }
    }
}
