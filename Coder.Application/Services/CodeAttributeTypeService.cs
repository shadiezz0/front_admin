using AutoMapper;
using Azure;
using Coder.Application.DTOs;
using Coder.Application.Helpers;
using Coder.Application.IServices;
using Coder.Domain.Entities;
using Coder.Domain.Interfaces;


namespace Coder.Application.Services
{
    public class CodeAttributeTypeService : ICodeAttributeTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CodeAttributeTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ApiResponse<CodeAttributeTypeDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.CodeAttributeTypes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeAttributeTypeDto>.NotFound("Code Attribute Type not found");

                var dto = _mapper.Map<CodeAttributeTypeDto>(entity);
                return ApiResponse<CodeAttributeTypeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeTypeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeAttributeTypeDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.CodeAttributeTypes.GetAllAsync();
                var dtos = _mapper.Map<List<CodeAttributeTypeDto>>(entities);
                return ApiResponse<List<CodeAttributeTypeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeAttributeTypeDto>>.InternalServerError(ex.Message);
            }
        }



   

        public async Task<ApiResponse<CodeAttributeTypeDto>> CreateAsync(CreateCodeAttributeTypeDto dto)
        {
            try
            {
                var entity = _mapper.Map<CodeAttributeType>(dto);
                var result = await _unitOfWork.CodeAttributeTypes.AddAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeTypeDto>(result);

                return ApiResponse<CodeAttributeTypeDto>.Created(resultDto, "Code Attribute Type created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeTypeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeTypeDto>> UpdateAsync(int id, UpdateCodeAttributeTypeDto dto)
        {
            try
            {
                var entity = await _unitOfWork.CodeAttributeTypes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeAttributeTypeDto>.NotFound("Code Attribute Type not found");

                _mapper.Map(dto, entity);
                var result = await _unitOfWork.CodeAttributeTypes.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeAttributeTypeDto>(result);

                return ApiResponse<CodeAttributeTypeDto>.Success(resultDto, "Code Attribute Type updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeTypeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeAttributeTypeDto>> DeleteAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.CodeAttributeTypes.DeleteAsync(id);
                if (!result)
                    return ApiResponse<CodeAttributeTypeDto>.NotFound("Code Attribute Type not found");

                return ApiResponse<CodeAttributeTypeDto>.Success(null, "Code Attribute Type deleted successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeAttributeTypeDto>.InternalServerError(ex.Message);
            }
        }
    }
}