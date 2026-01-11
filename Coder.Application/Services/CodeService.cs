using AutoMapper;
using Azure;
using Coder.Application.DTOs;
using Coder.Application.Helpers;
using Coder.Application.IServices;
using Coder.Domain.Entities;
using Coder.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.Services
{
    public class CodeService : ICodeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CodeService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<CodeDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Codes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeDto>.NotFound("Code not found");

                var dto = _mapper.Map<CodeDto>(entity);
                return ApiResponse<CodeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.Codes.GetAllAsync();
                var dtos = _mapper.Map<List<CodeDto>>(entities);
                return ApiResponse<List<CodeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeDto>>> GetByCodeTypeAsync(int codeTypeId)
        {
            try
            {
                var codeType = await _unitOfWork.CodeTypes.GetByIdAsync(codeTypeId);
                if (codeType == null)
                    return ApiResponse<List<CodeDto>>.BadRequest("Code Type not found");

                var entities = await _unitOfWork.Codes.FindAsync(x => x.CodeTypeId == codeTypeId);
                var dtos = _mapper.Map<List<CodeDto>>(entities);
                return ApiResponse<List<CodeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeDto>>> GetByStatusAsync(string status)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(status))
                    return ApiResponse<List<CodeDto>>.BadRequest("Status is required");

                var validStatuses = new[] { "DRAFT", "APPROVED", "INACTIVE" };
                if (!validStatuses.Contains(status.ToUpper()))
                    return ApiResponse<List<CodeDto>>.BadRequest("Invalid status");

                var entities = await _unitOfWork.Codes.FindAsync(x => x.Status == status.ToUpper());
                var dtos = _mapper.Map<List<CodeDto>>(entities);
                return ApiResponse<List<CodeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeDto>>.InternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// Generates code based on CodeTypeSettings (sort order), attribute details, and optional sequence.
        /// Steps:
        /// 1. Fetch CodeTypeSettings ordered by SortOrder
        /// 2. Validate all required settings are configured
        /// 3. Fetch CodeAttributeDetails values for each setting
        /// 4. Build code sections in sorted order
        /// 5. Get separator from first CodeTypeSetting
        /// 6. Join sections with separator
        /// 7. Append sequence if configured
        /// </summary>
        private async Task<string> GenerateCodeAsync(int codeTypeId)
        {
            try
            {
                // Step 1: Get CodeTypeSettings ordered by SortOrder (ascending)
                var codeTypeSettings = await _unitOfWork.CodeTypeSettings.FindAsync(
                    x => x.CodeTypeId == codeTypeId
                );

                if (!codeTypeSettings.Any())
                    throw new Exception($"No CodeTypeSettings configured for CodeType ID: {codeTypeId}");

                // Sort by SortOrder (ascending) - this determines the order of code sections
                var sortedSettings = codeTypeSettings.OrderBy(x => x.SortOrder).ToList();

                Console.WriteLine($"[CodeService] Found {sortedSettings.Count} CodeTypeSettings for CodeTypeId: {codeTypeId}");
                foreach (var setting in sortedSettings)
                {
                    Console.WriteLine($"  - AttributeDetailId: {setting.AttributeDetailId}, SortOrder: {setting.SortOrder}, Separator: {setting.Separator}, IsRequired: {setting.IsRequired}");
                }

                // Step 2: Validate all required settings are configured
                var missingRequiredSettings = sortedSettings.Where(x => x.IsRequired).ToList();
                if (!missingRequiredSettings.Any())
                    throw new Exception($"No required CodeTypeSettings found for CodeType ID: {codeTypeId}");

                // Step 3: Build code sections in sorted order by fetching CodeAttributeDetails
                var codeSections = new List<string>();

                foreach (var setting in sortedSettings)
                {
                    // Get the CodeAttributeDetails
                    var attributeDetail = await _unitOfWork.CodeAttributeDetails.GetByIdAsync(setting.AttributeDetailId);

                    if (attributeDetail == null)
                        throw new Exception($"CodeAttributeDetail not found for AttributeDetailId: {setting.AttributeDetailId}");

                    // Use the Code value from CodeAttributeDetails
                    var sectionValue = attributeDetail.Code;

                    if (string.IsNullOrWhiteSpace(sectionValue))
                        throw new Exception($"Code value cannot be empty for AttributeDetailId: {setting.AttributeDetailId}");

                    codeSections.Add(sectionValue.Trim());
                    Console.WriteLine($"[CodeService] Added section - AttributeDetailId: {setting.AttributeDetailId}, Code: {sectionValue.Trim()}, SortOrder: {setting.SortOrder}");
                }

                // Step 4: Get separator from first setting (all settings of same CodeType should share the same separator)
                var separator = sortedSettings.FirstOrDefault()?.Separator ?? "-";
                Console.WriteLine($"[CodeService] Using separator: '{separator}'");

                // Step 5: Join sections with separator (base code without sequence)
                var baseCode = string.Join(separator, codeSections);
                Console.WriteLine($"[CodeService] Base code generated: {baseCode}");

                // Step 6: ⭐ APPEND SEQUENCE if configured
                var sequence = await _unitOfWork.CodeTypeSequences
                    .FirstOrDefaultAsync(x => x.CodeTypeId == codeTypeId);

                if (sequence != null)
                {
                    Console.WriteLine($"[CodeService] Sequence found - CurrentValue: {sequence.CurrentValue}, MinValue: {sequence.MinValue}, MaxValue: {sequence.MaxValue}, IsCycling: {sequence.IsCycling}");

                    var nextValue = sequence.CurrentValue + 1;

                    // Handle cycling
                    if (nextValue > sequence.MaxValue)
                    {
                        if (sequence.IsCycling > 0)
                        {
                            nextValue = sequence.MinValue + sequence.StartWith;
                            Console.WriteLine($"[CodeService] Sequence cycled - NextValue reset to: {nextValue}");
                        }
                        else
                        {
                            throw new Exception(
                                $"Sequence has reached maximum value ({sequence.MaxValue}). Enable cycling to continue.");
                        }
                    }

                    // Format sequence value with leading zeros based on IsCycling (total width)
                    string nextValueStr = nextValue.ToString();
                    int zerosCount = sequence.IsCycling - nextValueStr.Length;
                    if (zerosCount < 0)
                        zerosCount = 0;

                    string formattedSequence = new string('0', zerosCount) + nextValueStr;

                    // Append sequence with separator
                    var finalCode = $"{baseCode}{separator}{formattedSequence}";

                    // Update sequence current value
                    sequence.CurrentValue = nextValue;
                    await _unitOfWork.CodeTypeSequences.UpdateAsync(sequence);

                    Console.WriteLine($"[CodeService] Final code with sequence: {finalCode} (SequenceValue: {formattedSequence}, NextCurrentValue: {nextValue})");
                    return finalCode;
                }
                else
                {
                    Console.WriteLine($"[CodeService] No sequence configured - returning base code: {baseCode}");
                    return baseCode;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CodeService] Error in GenerateCodeAsync: {ex.Message}");
                throw new Exception($"Error generating code: {ex.Message}");
            }
        }

        public async Task<ApiResponse<CodeDto>> CreateAsync(CreateCodeDto dto)
        {
            try
            {
                // Step 1: Validate CodeType exists
                var codeType = await _unitOfWork.CodeTypes.GetByIdAsync(dto.CodeTypeId);
                if (codeType == null)
                    return ApiResponse<CodeDto>.BadRequest("Code Type does not exist");

                // Step 2: AUTO-GENERATE code with sequence using private method
                string generatedCode;
                try
                {
                    generatedCode = await GenerateCodeAsync(dto.CodeTypeId);
                }
                catch (Exception ex)
                {
                    return ApiResponse<CodeDto>.BadRequest($"Failed to generate code: {ex.Message}");
                }

                // Step 3: Check if code already exists
                var exists = await _unitOfWork.Codes.AnyAsync(x =>
                    x.CodeTypeId == dto.CodeTypeId && x.CodeGenerated == generatedCode);
                if (exists)
                    return ApiResponse<CodeDto>.Conflict($"Code '{generatedCode}' already exists for this Code Type");

                // Step 4: Map DTO to entity and set generated code
                var entity = _mapper.Map<Code>(dto);
                entity.CodeGenerated = generatedCode;
                entity.CreatedBy = _currentUserService.GetUserName();
                entity.Status = "DRAFT";
                entity.CreatedAt = DateTime.Now;

                // Step 5: Save to database
                var result = await _unitOfWork.Codes.AddAsync(entity);
                var resultDto = _mapper.Map<CodeDto>(result);

                return ApiResponse<CodeDto>.Created(
                    resultDto,
                    $"Code created successfully. Generated code: {generatedCode}");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeDto>> UpdateAsync(int id, UpdateCodeDto dto)
        {
            try
            {
                var entity = await _unitOfWork.Codes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeDto>.NotFound("Code not found");

                if (!string.IsNullOrWhiteSpace(dto.Status))
                {
                    var validStatuses = new[] { "DRAFT", "APPROVED", "INACTIVE" };
                    if (!validStatuses.Contains(dto.Status.ToUpper()))
                        return ApiResponse<CodeDto>.BadRequest("Invalid status");
                }

                _mapper.Map(dto, entity);
                var result = await _unitOfWork.Codes.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeDto>(result);

                return ApiResponse<CodeDto>.Success(resultDto, "Code updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeDto>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Codes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeDto>.NotFound("Code not found");

                var result = await _unitOfWork.Codes.DeleteAsync(entity);
                if (!result)
                    return ApiResponse<CodeDto>.BadRequest("Failed to delete Code");

                return ApiResponse<CodeDto>.Success(null, "Code deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeDto>> ApproveAsync(int id, string approvedBy)
        {
            try
            {
                var entity = await _unitOfWork.Codes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeDto>.NotFound("Code not found");

                entity.Status = "APPROVED";
                entity.ApprovedAt = DateTime.Now;
                entity.ApprovedBy = _currentUserService.GetUserName();

                var result = await _unitOfWork.Codes.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeDto>(result);
                return ApiResponse<CodeDto>.Success(resultDto, "Code approved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeDto>> RejectAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.Codes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeDto>.NotFound("Code not found");

                if (entity.Status == "APPROVED")
                    return ApiResponse<CodeDto>.Conflict("Cannot reject an approved code");

                entity.Status = "INACTIVE";

                var result = await _unitOfWork.Codes.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeDto>(result);

                return ApiResponse<CodeDto>.Success(resultDto, "Code rejected successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeDto>.InternalServerError(ex.Message);
            }
        }
    }
}