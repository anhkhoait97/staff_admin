using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.New;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class NewsController : AuthControllerBase 
    { 
        private readonly ILogger<NewsController> _logger;
        private readonly INewsServices _newsServices;

        public NewsController(
            ILogger<NewsController> logger,
            INewsServices newsServices)
        {
            _logger = logger;
            _newsServices = newsServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsServices.GetById(id);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListNewsDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsServices.GetLists(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Created([FromBody] CreatedNewsDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsServices.Create(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] CreatedNewsDto data)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsServices.Update(data);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }
    }
}