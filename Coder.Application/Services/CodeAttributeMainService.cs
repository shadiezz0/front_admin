using AutoMapper;
using Azure;
using Coder.Application.DTOs;
using Coder.Application.Helpers;
using Coder.Application.IServices;
using Coder.Domain.Entities;
using Coder.Domain.Interfaces;

namespace Coder.Application.Services
{
    public class CodeAttributeMainService : ICodeAttributeMainService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CodeAttributeMainService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CodeAttributeMainDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.CodeAttributeMains.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeAttributeMainDto>.NotFound("Code Attribute Main not found");

                var dto = _mapper.Map<CodeAttributeMainDto>(entity);
                return ApiResponse<CodeAttributeMainDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeMainDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeAttributeMainDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.CodeAttributeMains.GetAllAsync();
                var dtos = _mapper.Map<List<CodeAttributeMainDto>>(entities);
                return ApiResponse<List<CodeAttributeMainDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeAttributeMainDto>>.InternalServerError(ex.Message);
            }
        }



        public async Task<ApiResponse<List<CodeAttributeMainDto>>> GetByCodeAttributeTypeAsync(int codeAttributeTypeId)
        {
            try
            {
                var entities = await _unitOfWork.CodeAttributeMains.FindAsync(x => x.CodeAttributeTypeId == codeAttributeTypeId);
                var dtos = _mapper.Map<List<CodeAttributeMainDto>>(entities);
                return ApiResponse<List<CodeAttributeMainDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeAttributeMainDto>>.InternalServerError(ex.Message);
            }

        }

        public async Task<ApiResponse<CodeAttributeMainDto>> CreateAsync(CreateCodeAttributeMainDto dto)
        {
            try
            {
                var exists = await _unitOfWork.CodeAttributeMains.AnyAsync(x =>
                    x.CodeAttributeTypeId == dto.CodeAttributeTypeId && x.Code == dto.Code);
                if (exists)
                    return ApiResponse<CodeAttributeMainDto>.Conflict("Code already exists for this Attribute Type");

                var entity = _mapper.Map<CodeAttributeMain>(dto);
                var result = await _unitOfWork.CodeAttributeMains.AddAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeMainDto>(result);

                return ApiResponse<CodeAttributeMainDto>.Created(resultDto, "Code Attribute Main created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeMainDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeMainDto>> UpdateAsync(int id, UpdateCodeAttributeMainDto dto)
        {
            try
            {
                var entity = await _unitOfWork.CodeAttributeMains.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeAttributeMainDto>.NotFound("Code Attribute Main not found");

                _mapper.Map(dto, entity);
                var result = await _unitOfWork.CodeAttributeMains.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeMainDto>(result);

                return ApiResponse<CodeAttributeMainDto>.Success(resultDto, "Code Attribute Main updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeMainDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeMainDto>> DeleteAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.CodeAttributeMains.DeleteAsync(id);
                if (!result)
                    return ApiResponse<CodeAttributeMainDto>.NotFound("Code Attribute Main not found");

                return ApiResponse<CodeAttributeMainDto>.Success(null,"Code Attribute Main deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeMainDto>.InternalServerError(ex.Message);
            }
        }
    }
}
