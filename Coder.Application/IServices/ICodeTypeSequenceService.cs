using Azure;
using Coder.Application.DTOs;
using Coder.Application.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.IServices
{
    public interface ICodeTypeSequenceService
    {
        Task<ApiResponse<CodeTypeSequenceDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeTypeSequenceDto>>> GetAllAsync();
        Task<ApiResponse<CodeTypeSequenceDto>> GetByCodeTypeAsync(int codeTypeId);
        Task<ApiResponse<CodeTypeSequenceDto>> CreateAsync(CreateCodeTypeSequenceDto dto);
        Task<ApiResponse<CodeTypeSequenceDto>> UpdateAsync(int id, UpdateCodeTypeSequenceDto dto);
        Task<ApiResponse<CodeTypeSequenceDto>> DeleteAsync(int id);
        Task<ApiResponse<object>> GetNextSequenceValueAsync(int codeTypeId);
    }
}
