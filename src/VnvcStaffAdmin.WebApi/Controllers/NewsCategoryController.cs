using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VnvcStaffAdmin.Application.Services.Interfaces;
using VnvcStaffAdmin.Domain.Dtos.Elasticsearchs;
using VnvcStaffAdmin.Domain.Dtos.NewCategories;
using VnvcStaffAdmin.Domain.Model;

namespace VnvcStaffAdmin.WebApi.Controllers
{
    [AllowAnonymous]
    public class NewsCategoryController : AuthControllerBase
    {
        private readonly INewsCategoryService _newsCategoryService;

        public NewsCategoryController(INewsCategoryService newsCategoryService)
        {
            _newsCategoryService = newsCategoryService;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetById([FromQuery] string id)
        {
            var result = await _newsCategoryService.GetByIdAsync(id);
            return Ok(result);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType<DatasourceResult<NewsCategory>>(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLists([FromQuery] QueryGetListNewsCategoryDto query)
        {
            if (ModelState.IsValid)
            {
                var response = await _newsCategoryService.GetLists(query);
                return Ok(response);
            }
            return BadRequest(ModelState);
        }

        /// <summary>
        /// GetByQuery
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> GetByQuery([FromBody] ElasticSearchQuery query)
        {
            var result = await _newsCategoryService.GetByQuery(query);
            return Ok(result);
        }

        /// <summary>
        /// Create
        /// </summary>
        /// <param name="data"></param>
        /// <response code="200"></response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateNewCategoryDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _newsCategoryService.AddAsync(data);
            return Ok(response);
        }

        /// <summary>
        /// update
        /// </summary>
        /// <param name="data"></param>
        /// <response code="200"></response>
        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UpdateNewsCategoryDto data)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _newsCategoryService.UpdateAsync(data);
            return Ok(response);
        }
    }
}