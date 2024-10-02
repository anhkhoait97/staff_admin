using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.Recruitments;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/staff-admin/[controller]/[action]")]
    public class RecruitmentController : ControllerBase
    {
        private readonly ILogger<RecruitmentController> _logger;
        private readonly IRecruitmentService _jobPostService;

        public RecruitmentController(
            ILogger<RecruitmentController> logger,
            IRecruitmentService jobPostService)
        {
            _logger = logger;
            _jobPostService = jobPostService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] CreateRecruitmentDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _jobPostService.CreateAsync(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _jobPostService.GetByIdAsync(id);
                return Ok(result);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateRecruitmentDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _jobPostService.UpdateAsync(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}