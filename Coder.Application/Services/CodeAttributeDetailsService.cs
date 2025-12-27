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

        public CodeAttributeDetailsService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                var exists = await _unitOfWork.CodeAttributeDetails.AnyAsync(x =>
                    x.AttributeMainId == dto.AttributeMainId && x.Code == dto.Code);
                if (exists)
                    return ApiResponse<CodeAttributeDetailsDto>.Conflict("Code already exists for this Attribute Main");

                var entity = _mapper.Map<CodeAttributeDetails>(dto);
                var result = await _unitOfWork.CodeAttributeDetails.AddAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeDetailsDto>(result);

                return ApiResponse<CodeAttributeDetailsDto>.Created(resultDto, "Code Attribute Details created successfully");
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
    }
}
