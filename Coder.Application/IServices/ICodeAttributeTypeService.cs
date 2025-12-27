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
    public interface ICodeAttributeTypeService
    {
        Task<ApiResponse<CodeAttributeTypeDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeAttributeTypeDto>>> GetAllAsync();
        Task<ApiResponse<CodeAttributeTypeDto>> CreateAsync(CreateCodeAttributeTypeDto dto);
        Task<ApiResponse<CodeAttributeTypeDto>> UpdateAsync(int id, UpdateCodeAttributeTypeDto dto);
        Task<ApiResponse<CodeAttributeTypeDto>> DeleteAsync(int id);
    }
}
