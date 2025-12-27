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
    public interface ICodeService
    {
        Task<ApiResponse<CodeDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeDto>>> GetAllAsync();
        Task<ApiResponse<List<CodeDto>>> GetByCodeTypeAsync(int codeTypeId);
        Task<ApiResponse<List<CodeDto>>> GetByStatusAsync(string status);
        Task<ApiResponse<CodeDto>> CreateAsync(CreateCodeDto dto);
        Task<ApiResponse<CodeDto>> UpdateAsync(int id, UpdateCodeDto dto);
        Task<ApiResponse<CodeDto>> DeleteAsync(int id);
        Task<ApiResponse<CodeDto>> ApproveAsync(int id, string approvedBy);
        Task<ApiResponse<CodeDto>> RejectAsync(int id);
    }
}
