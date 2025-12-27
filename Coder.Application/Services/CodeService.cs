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

        public CodeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
                var entities = await _unitOfWork.Codes.FindAsync(x => x.Status == status);
                var dtos = _mapper.Map<List<CodeDto>>(entities);
                return ApiResponse<List<CodeDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ApiResponse<List<CodeDto>>.InternalServerError(ex.Message);
            }
        }

     

        public async Task<ApiResponse<CodeDto>> CreateAsync(CreateCodeDto dto)
        {
            try
            {
                var exists = await _unitOfWork.Codes.AnyAsync(x =>
                    x.CodeTypeId == dto.CodeTypeId && x.CodeGenerated == dto.CodeGenerated);
                if (exists)
                    return ApiResponse<CodeDto>.Conflict("Code already exists");

                var entity = _mapper.Map<Code>(dto);
                var result = await _unitOfWork.Codes.AddAsync(entity);
                var resultDto = _mapper.Map<CodeDto>(result);

                return ApiResponse<CodeDto>.Created(resultDto, "Code created successfully");
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
                var result = await _unitOfWork.Codes.DeleteAsync(id);
                if (!result)
                    return ApiResponse<CodeDto>.NotFound("Code not found");

                return ApiResponse<CodeDto>.Success(null,"Code deleted successfully");
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
                entity.ApprovedBy = approvedBy;
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
