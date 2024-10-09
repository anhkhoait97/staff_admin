using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.Banners;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class BannerController : AuthControllerBase
    {
        private readonly ILogger<BannerController> _logger;
        private readonly IBannerService _bannerService;

        public BannerController(
            ILogger<BannerController> logger,
            IBannerService bannerService)
        {
            _logger = logger;
            _bannerService = bannerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (ModelState.IsValid)
            {
                var response = await _bannerService.GetById(id);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListBannerDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _bannerService.GetLists(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Created([FromBody] CreatedBannerDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _bannerService.Create(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CreatedBannerDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _bannerService.Update(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}