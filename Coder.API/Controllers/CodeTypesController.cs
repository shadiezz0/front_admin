using Azure;
using Coder.Application.DTOs;
using Coder.Application.Helpers;
using Coder.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CodeTypesController : ControllerBase
    {
        private readonly ICodeTypeService _service;

        public CodeTypesController(ICodeTypeService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeTypeDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeTypeDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

       

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeTypeDto>>> CreateAsync([FromBody] CreateCodeTypeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeTypeDto>>> UpdateAsync(int id, [FromBody] UpdateCodeTypeDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeTypeDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(template: "Approve")]
        public async Task<ActionResult<ApiResponse<CodeTypeDto>>> ApproveAsync(int id, [FromQuery] string approvedBy)
        {
            var result = await _service.ApproveAsync(id, approvedBy);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
    
