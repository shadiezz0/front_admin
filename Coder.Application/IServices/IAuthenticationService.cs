using Coder.Application.DTOs.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.IServices
{
    public interface IAuthenticationService
    { 
        Task<AuthTokenResponse> LoginAsync(string userCode, string password);
        Task<string> GetCurrentUserIdAsync();
    }
}
