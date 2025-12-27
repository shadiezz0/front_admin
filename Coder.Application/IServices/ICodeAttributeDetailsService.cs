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
    public interface ICodeAttributeDetailsService
    {
        Task<ApiResponse<CodeAttributeDetailsDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<CodeAttributeDetailsDto>>> GetAllAsync();
        Task<ApiResponse<List<CodeAttributeDetailsDto>>> GetByAttributeMainAsync(int attributeMainId);
        Task<ApiResponse<List<CodeAttributeDetailsDto>>> GetChildDetailsAsync(int parentDetailId);
        Task<ApiResponse<CodeAttributeDetailsDto>> CreateAsync(CreateCodeAttributeDetailsDto dto);
        Task<ApiResponse<CodeAttributeDetailsDto>> UpdateAsync(int id, UpdateCodeAttributeDetailsDto dto);
        Task<ApiResponse<CodeAttributeDetailsDto>> DeleteAsync(int id);
    }
}
