using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Domain.Model;
using VnvcStaffAdmin.Identity.Dtos.ApplicationUsers;
using VnvcStaffAdmin.Identity.Models;
using VnvcStaffAdmin.Identity.Services;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class AdminAccountController : AuthControllerBase
    {
        private readonly ILogger<AdminAccountController> _logger;
        private readonly IApplicationUserService _appUserService;

        public AdminAccountController(
            ILogger<AdminAccountController> logger,
            IApplicationUserService appUserService)
        {
            _logger = logger;
            _appUserService = appUserService;
        }

        [HttpGet]
        [ProducesResponseType<ResponseModel<ApplicationUser>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (ModelState.IsValid)
            {
                var response = await _appUserService.GetById(id);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [ProducesResponseType<DatasourceResult<ApplicationUser>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListApplicationUserDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _appUserService.GetLists(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        [ProducesResponseType<ResponseModel<ApplicationUser>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> Update([FromBody] UpdateApplicationUserDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _appUserService.Update(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}