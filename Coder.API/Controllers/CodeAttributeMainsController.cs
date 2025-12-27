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
    public class CodeAttributeMainsController : ControllerBase
    {
        private readonly ICodeAttributeMainService _service;

        public CodeAttributeMainsController(ICodeAttributeMainService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeAttributeMainDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeAttributeMainDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

  
        [HttpGet(template: "GetByCodeAttributeType")]
        public async Task<ActionResult<ApiResponse<List<CodeAttributeMainDto>>>> GetByCodeAttributeTypeAsync(int codeAttributeTypeId)
        {
            var result = await _service.GetByCodeAttributeTypeAsync(codeAttributeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

     

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeAttributeMainDto>>> CreateAsync([FromBody] CreateCodeAttributeMainDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeAttributeMainDto>>> UpdateAsync(int id, [FromBody] UpdateCodeAttributeMainDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeAttributeMainDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
    