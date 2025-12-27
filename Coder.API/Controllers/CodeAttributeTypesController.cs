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
    public class CodeAttributeTypesController : ControllerBase
    {
        private readonly ICodeAttributeTypeService _service;

        public CodeAttributeTypesController(ICodeAttributeTypeService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeAttributeTypeDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeAttributeTypeDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }


       

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeAttributeTypeDto>>> CreateAsync([FromBody] CreateCodeAttributeTypeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeAttributeTypeDto>>> UpdateAsync(int id, [FromBody] UpdateCodeAttributeTypeDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeAttributeTypeDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

    
