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
    public interface ICodeTypeService
    {
        Task<ApiResponse<CodeTypeDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeTypeDto>>> GetAllAsync();
        Task<ApiResponse<CodeTypeDto>> CreateAsync(CreateCodeTypeDto dto);
        Task<ApiResponse<CodeTypeDto>> UpdateAsync(int id, UpdateCodeTypeDto dto);
        Task<ApiResponse<CodeTypeDto>> DeleteAsync(int id);
        Task<ApiResponse<CodeTypeDto>> ApproveAsync(int id, string approvedBy);
    }
}
