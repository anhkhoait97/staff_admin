using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    [Authorize]
    public abstract class AuthControllerBase : CustomControllerBase
    {
        // Constructor to inject logger
        protected AuthControllerBase()
        {
        }

        protected bool IsAuthenticated()
        {
            return User.Identity?.IsAuthenticated ?? false;
        }
        protected string? GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
        protected string? GetUsername()
        {
            return User.Identity?.Name;
        }
        protected string[] GetUserRoles()
        {
            return User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();
        }
        protected bool IsInRole(string role)
        {
            return User.IsInRole(role);
        }
        protected bool HasAnyRole(params string[] roles)
        {
            return roles.Any(role => IsInRole(role));
        }
        protected IActionResult AuthorizeRole(string role)
        {
            if (!IsAuthenticated())
            {
                return UnauthorizedAccess("User is not authenticated.");
            }

            if (!IsInRole(role))
            {
                return ForbiddenAccess($"User does not have the required role: {role}.");
            }

            return SuccessResponse(null, $"User is authorized with role: {role}.");
        }

    }
}