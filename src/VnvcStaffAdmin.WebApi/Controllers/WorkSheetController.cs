using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Helpers;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.WorkSheets;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    public class WorkSheetController : AuthControllerBase
    {
        private readonly ILogger<WorkSheetController> _logger;
        private readonly IWorkSheetService _workSheetService;

        public WorkSheetController(
            ILogger<WorkSheetController> logger,
            IWorkSheetService workSheetService)
        {
            _logger = logger;
            _workSheetService = workSheetService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkSheets([FromQuery] QueryGetListWorkSheetDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _workSheetService.GetWorkSheets(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> GetDetailWorkSheetOfUser([FromQuery] QueryGetDetailWorkSheetHistoryDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _workSheetService.GetDetailWorkSheetOfUser(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        [HttpGet]
        public async Task<IActionResult> ExportWorkSheets([FromQuery] QueryGetListWorkSheetDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _workSheetService.ExportWorkSheets(query);

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

        [HttpGet]
        public async Task<IActionResult> ExportDetailWorkSheetOfUser([FromQuery] QueryGetDetailWorkSheetHistoryDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _workSheetService.ExportDetailWorkSheetOfUser(query);

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
    }
}