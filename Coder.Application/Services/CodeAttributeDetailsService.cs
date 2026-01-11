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
    public class CodeAttributeDetailsService : ICodeAttributeDetailsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CodeAttributeDetailsService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<CodeAttributeDetailsDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.CodeAttributeDetails.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeAttributeDetailsDto>.NotFound("Code Attribute Details not found");

                var dto = _mapper.Map<CodeAttributeDetailsDto>(entity);
                return ApiResponse<CodeAttributeDetailsDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeDetailsDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeAttributeDetailsDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.CodeAttributeDetails.GetAllAsync();
                var dtos = _mapper.Map<List<CodeAttributeDetailsDto>>(entities);
                return ApiResponse<List<CodeAttributeDetailsDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeAttributeDetailsDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeAttributeDetailsDto>>> GetByAttributeMainAsync(int attributeMainId)
        {
            try
            {
                var entities = await _unitOfWork.CodeAttributeDetails.FindAsync(x => x.AttributeMainId == attributeMainId);
                var dtos = _mapper.Map<List<CodeAttributeDetailsDto>>(entities);
                return ApiResponse<List<CodeAttributeDetailsDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeAttributeDetailsDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeAttributeDetailsDto>>> GetChildDetailsAsync(int parentDetailId)
        {
            try
            {
                var entities = await _unitOfWork.CodeAttributeDetails.FindAsync(x => x.ParentDetailId == parentDetailId);
                var dtos = _mapper.Map<List<CodeAttributeDetailsDto>>(entities);
                return ApiResponse<List<CodeAttributeDetailsDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeAttributeDetailsDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeDetailsDto>> CreateAsync(CreateCodeAttributeDetailsDto dto)
        {
            try
            {
                // Step 1: Check if code already exists for this Attribute Main
                var exists = await _unitOfWork.CodeAttributeDetails.AnyAsync(x =>
                    x.AttributeMainId == dto.AttributeMainId && x.Code == dto.Code);
                if (exists)
                    return ApiResponse<CodeAttributeDetailsDto>.Conflict("Code already exists for this Attribute Main");

                // Step 2: Map DTO to entity
                var entity = _mapper.Map<CodeAttributeDetails>(dto);
                if (entity.ParentDetailId <= 0)
                    entity.ParentDetailId = null;

                entity.CreatedBy = _currentUserService.GetUserName();
                entity.CreatedAt = DateTime.Now;

                // Step 3: Save CodeAttributeDetails
                var result = await _unitOfWork.CodeAttributeDetails.AddAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeDetailsDto>(result);

                // Step 4: ⭐ AUTO-CREATE CodeTypeSetting for this new attribute detail
                await CreateCodeTypeSettingAsync(result, dto.sortOrder);

                return ApiResponse<CodeAttributeDetailsDto>.Created(resultDto, "Code Attribute Details created successfully and CodeTypeSetting auto-created");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeDetailsDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeDetailsDto>> UpdateAsync(int id, UpdateCodeAttributeDetailsDto dto)
        {
            try
            {
                var entity = await _unitOfWork.CodeAttributeDetails.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeAttributeDetailsDto>.NotFound("Code Attribute Details not found");

                _mapper.Map(dto, entity);
                var result = await _unitOfWork.CodeAttributeDetails.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeDetailsDto>(result);

                return ApiResponse<CodeAttributeDetailsDto>.Success(resultDto, "Code Attribute Details updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeDetailsDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeDetailsDto>> DeleteAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.CodeAttributeDetails.DeleteAsync(id);
                if (!result)
                    return ApiResponse<CodeAttributeDetailsDto>.NotFound("Code Attribute Details not found");

                return ApiResponse<CodeAttributeDetailsDto>.Success(null, "Code Attribute Details deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeDetailsDto>.InternalServerError(ex.Message);
            }
        }

        /// <summary>
        /// ⭐ Private method to auto-create CodeTypeSetting after attribute detail is created
        /// Gets the CodeTypeId from the parent CodeAttributeMain
        /// Creates appropriate CodeTypeSetting with default values
        /// </summary>
        private async Task CreateCodeTypeSettingAsync(CodeAttributeDetails attributeDetail, int sortOreder)
        {
            try
            {
                // Step 1: Get the parent CodeAttributeMain to access CodeTypeId
                var attributeMain = await _unitOfWork.CodeAttributeMains.GetByIdAsync(attributeDetail.AttributeMainId);
                if (attributeMain == null)
                {
                    Console.WriteLine($"[CodeAttributeDetailsService] Parent CodeAttributeMain not found for AttributeMainId: {attributeDetail.AttributeMainId}");
                    return;
                }

                int codeTypeId = attributeMain.CodeTypeId;

                // Step 2: Check if CodeTypeSetting already exists for this combination
                var existingSetting = await _unitOfWork.CodeTypeSettings.AnyAsync(x =>
                    x.CodeTypeId == codeTypeId && x.AttributeDetailId == attributeDetail.Id);

                if (existingSetting)
                {
                    Console.WriteLine($"[CodeAttributeDetailsService] CodeTypeSetting already exists for CodeTypeId: {codeTypeId}, AttributeDetailId: {attributeDetail.Id}");
                    return;
                }

                // Step 3: Get the highest SortOrder for this CodeType
                var existingSettings = await _unitOfWork.CodeTypeSettings.FindAsync(x => x.CodeTypeId == codeTypeId);
                //int nextSortOrder = existingSettings.Any() ? existingSettings.Max(x => x.SortOrder) + 1 : 1;

                // Step 4: Create new CodeTypeSetting with default values
                var newSetting = new CodeTypeSetting
                {
                    CodeTypeId = codeTypeId,
                    AttributeDetailId = attributeDetail.Id,
                    SortOrder = sortOreder,
                    Separator = "-", // Default separator
                    IsRequired = true, // Default as required
                    CreatedAt = DateTime.Now
                };

                // Step 5: Save CodeTypeSetting
                await _unitOfWork.CodeTypeSettings.AddAsync(newSetting);

                Console.WriteLine($"[CodeAttributeDetailsService] CodeTypeSetting auto-created: CodeTypeId={codeTypeId}, AttributeDetailId={attributeDetail.Id}, SortOrder={sortOreder}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CodeAttributeDetailsService] Error auto-creating CodeTypeSetting: {ex.Message}");
                // Don't throw - log and continue. The attribute detail was created successfully.
            }
        }
    }
}
