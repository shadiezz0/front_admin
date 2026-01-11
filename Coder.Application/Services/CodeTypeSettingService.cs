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
    public class CodeTypeSettingService : ICodeTypeSettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CodeTypeSettingService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<CodeTypeSettingDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.CodeTypeSettings.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeTypeSettingDto>.NotFound("Code Type Setting not found");

                var dto = _mapper.Map<CodeTypeSettingDto>(entity);
                return ApiResponse<CodeTypeSettingDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeSettingDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeTypeSettingDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.CodeTypeSettings.GetAllAsync();
                var dtos = _mapper.Map<List<CodeTypeSettingDto>>(entities);
                return ApiResponse<List<CodeTypeSettingDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeTypeSettingDto>>.InternalServerError(ex.Message);
            }
        }

     
        public async Task<ApiResponse<List<CodeTypeSettingDto>>> GetByCodeTypeAsync(int codeTypeId)
        {
            try
            {
                // Verify Code Type exists
                var codeType = await _unitOfWork.CodeTypes.GetByIdAsync(codeTypeId);
                if (codeType == null)
                    return ApiResponse<List<CodeTypeSettingDto>>.NotFound("Code Type not found");

                var entities = await _unitOfWork.CodeTypeSettings.FindAsync(x => x.CodeTypeId == codeTypeId);
                var dtos = _mapper.Map<List<CodeTypeSettingDto>>(entities);
                return ApiResponse<List<CodeTypeSettingDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeTypeSettingDto>>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeTypeSettingDto>> CreateAsync(CreateCodeTypeSettingDto dto)
        {
            try
            {
                // Validate Code Type exists
                var codeType = await _unitOfWork.CodeTypes.GetByIdAsync(dto.CodeTypeId);
                if (codeType == null)
                    return ApiResponse<CodeTypeSettingDto>.BadRequest("Code Type does not exist");

                // Validate Attribute Detail exists
                var attributeDetail = await _unitOfWork.CodeAttributeDetails.GetByIdAsync(dto.AttributeDetailId);
                if (attributeDetail == null)
                    return ApiResponse<CodeTypeSettingDto>.BadRequest("Attribute Detail does not exist");

                // Check for duplicate
                var exists = await _unitOfWork.CodeTypeSettings.AnyAsync(x =>
                    x.CodeTypeId == dto.CodeTypeId && x.AttributeDetailId == dto.AttributeDetailId);
                if (exists)
                    return ApiResponse<CodeTypeSettingDto>.Conflict("This setting already exists for this Code Type");

                var entity = _mapper.Map<CodeTypeSetting>(dto);
                var result = await _unitOfWork.CodeTypeSettings.AddAsync(entity);
                var resultDto = _mapper.Map<CodeTypeSettingDto>(result);

                return ApiResponse<CodeTypeSettingDto>.Created(resultDto, "Code Type Setting created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeSettingDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeTypeSettingDto>> UpdateAsync(int id, UpdateCodeTypeSettingDto dto)
        {
            try
            {
                var entity = await _unitOfWork.CodeTypeSettings.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeTypeSettingDto>.NotFound("Code Type Setting not found");

                _mapper.Map(dto, entity);
                var result = await _unitOfWork.CodeTypeSettings.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeTypeSettingDto>(result);

                return ApiResponse<CodeTypeSettingDto>.Success(resultDto, "Code Type Setting updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeSettingDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeTypeSettingDto>> DeleteAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.CodeTypeSettings.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeTypeSettingDto>.NotFound("Code Type Setting not found");

                var result = await _unitOfWork.CodeTypeSettings.DeleteAsync(entity);
                if (!result)
                    return ApiResponse<CodeTypeSettingDto>.BadRequest("Failed to delete Code Type Setting");

                return ApiResponse<CodeTypeSettingDto>.Success(null,"Code Type Setting deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeSettingDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeTypeSettingDto>> DeleteByCodeTypeAsync(int codeTypeId)
        {
            try
            {
                // Verify Code Type exists
                var codeType = await _unitOfWork.CodeTypes.GetByIdAsync(codeTypeId);
                if (codeType == null)
                    return ApiResponse<CodeTypeSettingDto>.NotFound("Code Type not found");

                var result = await _unitOfWork.CodeTypeSettings.DeleteRangeAsync(x => x.CodeTypeId == codeTypeId);
                if (!result) 
                    return ApiResponse<CodeTypeSettingDto>.NotFound("No settings found to delete");

                return ApiResponse<CodeTypeSettingDto>.Success(null,"Code Type Settings deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeSettingDto>.InternalServerError(ex.Message);
            }
        }
    }
}
    
