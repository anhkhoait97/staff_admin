using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.New;
using VnvcStaffAdmin.Domain.Dtos.Recruitments;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class RecruitmentController : AuthControllerBase
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

        [HttpGet]
        [ProducesResponseType<DatasourceResult<Recruitment>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListRecruitmentDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _jobPostService.GetLists(query);
                return Ok(response);
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