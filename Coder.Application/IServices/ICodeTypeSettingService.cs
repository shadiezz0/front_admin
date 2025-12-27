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
    public interface ICodeTypeSettingService
    {
        Task<ApiResponse<CodeTypeSettingDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeTypeSettingDto>>> GetAllAsync();
        Task<ApiResponse<List<CodeTypeSettingDto>>> GetByCodeTypeAsync(int codeTypeId);
        Task<ApiResponse<CodeTypeSettingDto>> CreateAsync(CreateCodeTypeSettingDto dto);
        Task<ApiResponse<CodeTypeSettingDto>> UpdateAsync(int id, UpdateCodeTypeSettingDto dto);
        Task<ApiResponse<CodeTypeSettingDto>> DeleteAsync(int id);
        Task<ApiResponse<CodeTypeSettingDto>> DeleteByCodeTypeAsync(int codeTypeId);
    }
}
