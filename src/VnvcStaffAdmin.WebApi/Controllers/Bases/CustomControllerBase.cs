using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Domain.Constants;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    [Route("api/staff-admin/[controller]/[action]")]
    [ApiController]
    public abstract class CustomControllerBase : ControllerBase
    {
        protected IActionResult SuccessResponse(string? message = "Operation successful.", object? data = null)
        {
            return Ok(ResponseModel.Successed(message, data));
        }

        protected IActionResult ErrorResponse(string error)
        {
            return BadRequest(ResponseModel.Failed(error));
        }

        protected IActionResult HandleException(Exception exception)
        {
            return StatusCode(500, ResponseModel.Failed("An internal server error occurred. Please try again later.", errorCode: ErrorCodes.InternalServerError));
        }

        /// <summary>
        /// Gets the client's IP address from the request.
        /// </summary>
        /// <returns>Client IP address</returns>
        protected string GetClientIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown IP";
        }

        /// <summary>
        /// Custom response for unauthorized access.
        /// </summary>
        /// <param name="message">Optional unauthorized message</param>
        /// <returns>401 Unauthorized result</returns>
        protected IActionResult UnauthorizedAccess(string? message = "Unauthorized access.")
        {
            return Unauthorized(ResponseModel.Failed(message, ErrorCodes.Unauthorized));
        }

        /// <summary>
        /// Custom response for forbidden access.
        /// </summary>
        /// <param name="message">Optional forbidden message</param>
        /// <returns>403 Forbidden result</returns>
        protected IActionResult ForbiddenAccess(string? message = "Forbidden access.")
        {
            return Forbid();
        }
    }
}