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
    public interface ICodeAttributeMainService
    {

        Task<ApiResponse<CodeAttributeMainDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeAttributeMainDto>>> GetAllAsync();
        Task<ApiResponse<List<CodeAttributeMainDto>>> GetByCodeAttributeTypeAsync(int codeAttributeTypeId);
        Task<ApiResponse<CodeAttributeMainDto>> CreateAsync(CreateCodeAttributeMainDto dto);
        Task<ApiResponse<CodeAttributeMainDto>> UpdateAsync(int id, UpdateCodeAttributeMainDto dto);
        Task<ApiResponse<CodeAttributeMainDto>> DeleteAsync(int id);
    }
}
