using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.ConfigStaffs;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class ConfigStaffController : AuthControllerBase
    {
        private readonly ILogger<ConfigStaffController> _logger;
        private readonly IConfigStaffService _configStaffService;

        public ConfigStaffController(
            ILogger<ConfigStaffController> logger,
            IConfigStaffService configStaffService)
        {
            _logger = logger;
            _configStaffService = configStaffService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (ModelState.IsValid)
            {
                var response = await _configStaffService.GetByIdAsync(id);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        [ProducesResponseType<DatasourceResult<ConfigStaff>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListConfigStaffDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _configStaffService.GetLists(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateConfigStaffDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _configStaffService.AddAsync(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateConfigStaffDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _configStaffService.UpdateAsync(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}