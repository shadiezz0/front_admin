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

        /// <summary>
        /// Get a code by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Invalid ID"));

            var result = await _service.GetByIdAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get all codes
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<CodeDto>>>> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get codes by Code Type ID
        /// </summary>
        [HttpGet("by-type/{codeTypeId}")]
        public async Task<ActionResult<ApiResponse<List<CodeDto>>>> GetByCodeTypeAsync(int codeTypeId)
        {
            if (codeTypeId <= 0)
                return BadRequest(ApiResponse<List<CodeDto>>.BadRequest("Invalid Code Type ID"));

            var result = await _service.GetByCodeTypeAsync(codeTypeId);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Get codes by status (DRAFT, APPROVED, INACTIVE)
        /// </summary>
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<ApiResponse<List<CodeDto>>>> GetByStatusAsync(string status)
        {
            var result = await _service.GetByStatusAsync(status);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Create a new code with auto-generated code value and optional sequence
        /// 
        /// The code generation process is FULLY AUTOMATED:
        /// 1. Fetches CodeTypeSettings for the specified CodeType (ordered by SortOrder)
        /// 2. Retrieves CodeAttributeDetails for each setting
        /// 3. Builds the base code by joining section codes according to SortOrder with the configured Separator
        /// 4. If a CodeTypeSequence is configured, appends the next sequence value to the final code
        /// 5. Stores the complete generated code in the CodeGenerated field
        /// 
        /// Example request:
        /// {
        ///   "codeTypeId": 1,
        ///   "nameAr": "خدمة كشف قلب",
        ///   "nameEn": "Cardiology Consultation",
        ///   "descriptionAr": "كشف قلب",
        ///   "descriptionEn": "Heart consultation",
        ///   "externalSystem": "HMS",
        ///   "externalReferenceId": "SRV-10001"
        /// }
        /// 
        /// Example response (assuming CodeTypeSettings configured with codes and sequence 000-999, width 3):
        /// {
        ///   "id": 1,
        ///   "codeTypeId": 1,
        ///   "nameAr": "خدمة كشف قلب",
        ///   "nameEn": "Cardiology Consultation",
        ///   "descriptionAr": "كشف قلب",
        ///   "descriptionEn": "Heart consultation",
        ///   "codeGenerated": "CAR-CONS-001",
        ///   "status": "DRAFT",
        ///   "externalSystem": "HMS",
        ///   "externalReferenceId": "SRV-10001",
        ///   "createdBy": "admin",
        ///   "createdAt": "2024-01-09T10:30:00",
        ///   "approvedAt": null,
        ///   "approvedBy": null
        /// }
        /// 
        /// Notes:
        /// - CodeGenerated is fully auto-generated and should NOT be provided in the request
        /// - Code generation is based entirely on CodeTypeSettings and CodeAttributeDetails
        /// - The separator is determined from CodeTypeSettings (e.g., "-", "/", etc.)
        /// - Sequence is optional and only appended if CodeTypeSequence is configured for this CodeType
        /// - Only CodeTypeId, NameAr/NameEn, and optionally Description/ExternalSystem are required
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ApiResponse<CodeDto>>> CreateAsync([FromBody] CreateCodeDto dto)
        {
            // Validate model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<CodeDto>.BadRequest($"Validation failed: {string.Join(", ", errors)}"));
            }

            // Validate DTO is not null
            if (dto == null)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Request body cannot be null"));

            // Validate CodeTypeId
            if (dto.CodeTypeId <= 0)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Invalid Code Type ID. CodeTypeId must be greater than 0"));

            // Validate required fields (either Arabic or English name)
            if (string.IsNullOrWhiteSpace(dto.NameEn) && string.IsNullOrWhiteSpace(dto.NameAr))
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Either NameEn or NameAr is required"));

            // Call service to create code (code generation is automatic based on CodeTypeSettings)
            var result = await _service.CreateAsync(dto);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Update code details (status, names, descriptions)
        /// Note: CodeGenerated and the underlying code structure cannot be changed after creation
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> UpdateAsync(int id, [FromBody] UpdateCodeDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(ApiResponse<CodeDto>.BadRequest($"Validation failed: {string.Join(", ", errors)}"));
            }

            if (id <= 0)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Invalid ID"));

            if (dto == null)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Request body cannot be null"));

            var result = await _service.UpdateAsync(id, dto);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Delete a code by ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> DeleteAsync(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Invalid ID"));

            var result = await _service.DeleteAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Approve a code (change status to APPROVED)
        /// </summary>
        [HttpPost("{id}/approve")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> ApproveAsync(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Invalid ID"));

            var result = await _service.ApproveAsync(id, string.Empty);
            return StatusCode((int)result.StatusCode, result);
        }

        /// <summary>
        /// Reject a code (change status to INACTIVE)
        /// </summary>
        [HttpPost("{id}/reject")]
        public async Task<ActionResult<ApiResponse<CodeDto>>> RejectAsync(int id)
        {
            if (id <= 0)
                return BadRequest(ApiResponse<CodeDto>.BadRequest("Invalid ID"));

            var result = await _service.RejectAsync(id);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}