using AutoMapper;
using Azure;
using Coder.Application.DTOs;
using Coder.Application.Helpers;
using Coder.Application.IServices;
using Coder.Domain.Entities;
using Coder.Domain.Interfaces;


namespace Coder.Application.Services
{
    public class CodeTypeService : ICodeTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;


        public CodeTypeService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ApiResponse<CodeTypeDto>> GetByIdAsync(int id)
        {
            try
            {
                var entity = await _unitOfWork.CodeTypes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeTypeDto>.NotFound("Code Type not found");

                var dto = _mapper.Map<CodeTypeDto>(entity);
                return ApiResponse<CodeTypeDto>.Success(dto);
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<List<CodeTypeDto>>> GetAllAsync()
        {
            try
            {
                var entities = await _unitOfWork.CodeTypes.GetAllAsync();
                var dtos = _mapper.Map<List<CodeTypeDto>>(entities);
                return ApiResponse<List<CodeTypeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeTypeDto>>.InternalServerError(ex.Message);
            }
        }




        public async Task<ApiResponse<CodeTypeDto>> CreateAsync(CreateCodeTypeDto dto)
        {
            try
            {
                var exists = await _unitOfWork.CodeTypes.AnyAsync(x =>
                    x.CodeTypeCode == dto.CodeTypeCode);
                if (exists)
                    return ApiResponse<CodeTypeDto>.Conflict("Code Type Code already exists");

                var entity = _mapper.Map<CodeType>(dto);

                // Set CreatedBy from current user
                entity.CreatedBy = _currentUserService.GetUserName();
                entity.CreatedAt = DateTime.Now;

                var result = await _unitOfWork.CodeTypes.AddAsync(entity);
                var resultDto = _mapper.Map<CodeTypeDto>(result);
                return ApiResponse<CodeTypeDto>.Created(resultDto, "Code Type created successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeTypeDto>> UpdateAsync(int id, UpdateCodeTypeDto dto)
        {
            try
            {
                var entity = await _unitOfWork.CodeTypes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeTypeDto>.NotFound("Code Type not found");

                _mapper.Map(dto, entity);
                var result = await _unitOfWork.CodeTypes.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeTypeDto>(result);

                return ApiResponse<CodeTypeDto>.Success(resultDto, "Code Type updated successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeDto>.InternalServerError(ex.Message);
            }
        }


        public async Task<ApiResponse<CodeTypeDto>> DeleteAsync(int id)
        {
            try
            {
                var result = await _unitOfWork.CodeTypes.DeleteAsync(id);
                if (!result)
                    return ApiResponse<CodeTypeDto>.NotFound("Code Type not found");

                return ApiResponse<CodeTypeDto>.Success(null, "Code Type deleted successfully"); ;
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeDto>.InternalServerError(ex.Message);
            }
        }

        public async Task<ApiResponse<CodeTypeDto>> ApproveAsync(int id, string approvedBy)
        {
            try
            {
                var entity = await _unitOfWork.CodeTypes.GetByIdAsync(id);
                if (entity == null)
                    return ApiResponse<CodeTypeDto>.NotFound("Code Type not found");

                entity.ApprovedAt = DateTime.Now;
                entity.ApprovedBy = _currentUserService.GetUserName();

                var result = await _unitOfWork.CodeTypes.UpdateAsync(entity);
                var resultDto = _mapper.Map<CodeTypeDto>(result);
                return ApiResponse<CodeTypeDto>.Success(resultDto, "Code Type approved successfully");
            }
            catch (Exception ex)
            {
                return ApiResponse<CodeTypeDto>.InternalServerError(ex.Message);
            }
        }
    }
}