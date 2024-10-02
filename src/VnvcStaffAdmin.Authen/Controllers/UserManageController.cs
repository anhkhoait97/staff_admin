using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Identity.Dtos.UserManage;
using VnvcStaffAdmin.Identity.Models;

namespace VnvcStaffAdmin.Authen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManageController : ControllerBase
    {
        private readonly ILogger<UserManageController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserManageController(
            ILogger<UserManageController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseModel.BadRequest(ModelState));

            var user = await _userManager.GetUserAsync(User);

            if (user == null) return Ok(ResponseModel.Failed($"Unable to load User with ID '{_userManager.GetUserId(User)}'"));

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, data.OldPassword, data.NewPassword);
            if (!changePasswordResult.Succeeded)
                return Ok(ResponseModel.Failed("Change password failed"));
            
            await _signInManager.SignInAsync(user, isPersistent: false);

            return Ok(ResponseModel.Successed("Change password successfully"));
        }
    }
}