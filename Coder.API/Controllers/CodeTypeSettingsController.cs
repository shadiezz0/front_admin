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
    public class CodeTypeSettingsController : ControllerBase
    {

        private readonly ICodeTypeSettingService _service;

        public CodeTypeSettingsController(ICodeTypeSettingService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeTypeSettingDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeTypeSettingDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

     

        [HttpGet(template: "GetByCodeType")]
        public async Task<ActionResult<ApiResponse<List<CodeTypeSettingDto>>>> GetByCodeTypeAsync(int codeTypeId)
        {
            var result = await _service.GetByCodeTypeAsync(codeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeTypeSettingDto>>> CreateAsync([FromBody] CreateCodeTypeSettingDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeTypeSettingDto>>> UpdateAsync(int id, [FromBody] UpdateCodeTypeSettingDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeTypeSettingDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "DeleteByCodeType")]
        public async Task<ActionResult<ApiResponse<CodeTypeSettingDto>>> DeleteByCodeTypeAsync(int codeTypeId)
        {
            var result = await _service.DeleteByCodeTypeAsync(codeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

    