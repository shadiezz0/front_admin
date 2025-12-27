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
    public class CodesController : ControllerBase
    {
        private readonly ICodeService _service;

        public CodesController(ICodeService service)
        {
            _service = service;
        }

        [HttpGet(template: "GetById")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> GetByIdAsync(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetAll")]
        public async Task<ActionResult<ApiResponse<List<CodeDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }


        [HttpGet(template: "GetByCodeType")]
        public async Task<ActionResult<ApiResponse<List<CodeDto>>>> GetByCodeTypeAsync(int codeTypeId)
        {
            var result = await _service.GetByCodeTypeAsync(codeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpGet(template: "GetByStatus")]
        public async Task<ActionResult<ApiResponse<List<CodeDto>>>> GetByStatusAsync(string status)
        {
            var result = await _service.GetByStatusAsync(status);
            return StatusCode((int)result.StatusCode, result);
        }

     

        [HttpPost(template: "Create")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> CreateAsync([FromBody] CreateCodeDto dto)
        {
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut(template: "Update")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> UpdateAsync(int id, [FromBody] UpdateCodeDto dto)
        {
            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpDelete(template: "Delete")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> DeleteAsync(int id)
        {
            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(template: "Approve")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> ApproveAsync(int id, [FromQuery] string approvedBy)
        {
            var result = await _service.ApproveAsync(id, approvedBy);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost(template: "Reject")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> RejectAsync(int id)
        {
            var result = await _service.RejectAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
    