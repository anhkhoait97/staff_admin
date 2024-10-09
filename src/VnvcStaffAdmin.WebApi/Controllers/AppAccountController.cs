using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Helpers;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.AppAccounts;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class AppAccountController : AuthControllerBase
    {
        private readonly ILogger<AppAccountController> _logger;
        private readonly IAppAccountService _appAccountService;

        public AppAccountController(
            ILogger<AppAccountController> logger,
            IAppAccountService appAccountService)
        {
            _logger = logger;
            _appAccountService = appAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (ModelState.IsValid)
            {
                var response = await _appAccountService.GetById(id);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListAppAccountDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _appAccountService.GetLists(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> Export([FromQuery] QueryGetListAppAccountDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _appAccountService.Export(query);

                var path = Path.Combine(
                           Directory.GetCurrentDirectory(),
                           "wwwroot/Export", response);

                var memory = new MemoryStream();
                using (var stream = new FileStream(path, FileMode.Open))
                {
                    await stream.CopyToAsync(memory);
                }
                memory.Position = 0;
                return File(memory, FileHelper.GetContentType(path), $"{Path.GetFileName(path)}");
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateAppAccountDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _appAccountService.Update(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}