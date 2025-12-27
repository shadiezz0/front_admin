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
    public class CodeAttributeDetailsController : ControllerBase
    {
        private readonly ICodeAttributeDetailsService _service;

        public CodeAttributeDetailsController(ICodeAttributeDetailsService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeAttributeDetailsDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeAttributeDetailsDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

  

        [HttpGet(template: "GetByAttributeMain")]
        public async Task<ActionResult<ApiResponse<List<CodeAttributeDetailsDto>>>> GetByAttributeMainAsync(int attributeMainId)
        {
            var result = await _service.GetByAttributeMainAsync(attributeMainId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetChildDetails")]
        public async Task<ActionResult<ApiResponse<List<CodeAttributeDetailsDto>>>> GetChildDetailsAsync(int parentDetailId)
        {
            var result = await _service.GetChildDetailsAsync(parentDetailId);
            return StatusCode((int)result.StatusCode, result);
        }

    

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeAttributeDetailsDto>>> CreateAsync([FromBody] CreateCodeAttributeDetailsDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeAttributeDetailsDto>>> UpdateAsync(int id, [FromBody] UpdateCodeAttributeDetailsDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeAttributeDetailsDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}

    