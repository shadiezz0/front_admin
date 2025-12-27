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
    public class CodeTypeSequencesController : ControllerBase
    {
        private readonly ICodeTypeSequenceService _service;

        public CodeTypeSequencesController(ICodeTypeSequenceService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeTypeSequenceDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeTypeSequenceDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

   

        [HttpGet(template: "GetByCodeType")]
        public async Task<ActionResult<ApiResponse<CodeTypeSequenceDto>>> GetByCodeTypeAsync(int codeTypeId)
        {
            var result = await _service.GetByCodeTypeAsync(codeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeTypeSequenceDto>>> CreateAsync([FromBody] CreateCodeTypeSequenceDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeTypeSequenceDto>>> UpdateAsync(int id, [FromBody] UpdateCodeTypeSequenceDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeTypeSequenceDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetNextSequenceValue")]
        public async Task<ActionResult<ApiResponse<object>>> GetNextSequenceValueAsync(int codeTypeId)
        {
            var result = await _service.GetNextSequenceValueAsync(codeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

    
