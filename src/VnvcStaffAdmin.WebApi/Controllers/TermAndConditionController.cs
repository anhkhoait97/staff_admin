using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.TermAndConditions;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class TermAndConditionController : AuthControllerBase
    {
        private readonly ILogger<TermAndConditionController> _logger;
        private readonly ITermAndConditionService _termAndConditionService;

        public TermAndConditionController(
            ILogger<TermAndConditionController> logger,
            ITermAndConditionService termAndConditionService)
        {
            _logger = logger;
            _termAndConditionService = termAndConditionService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateTermAndConditionDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _termAndConditionService.CreateOrUpdate(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetByType([FromQuery] string type)
        {
            if (ModelState.IsValid)
            {
                var result = await _termAndConditionService.GetByType(type);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }
    }
}