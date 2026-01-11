using Coder.Application.DTOs.Auth;
using Coder.Application.IServices;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private static string _currentUserId = "System";
        private static string _currentUserName = "System";
        private static string _currentToken = "";

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            try
            {
                var claim = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserId");
                if (!string.IsNullOrEmpty(claim?.Value))
                    return claim.Value;

                var userCodeClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserCode");
                if (!string.IsNullOrEmpty(userCodeClaim?.Value))
                    return userCodeClaim.Value;

                return !string.IsNullOrEmpty(_currentUserId) ? _currentUserId : "System";
            }
            catch
            {
                return !string.IsNullOrEmpty(_currentUserId) ? _currentUserId : "System";
            }
        }

        public string GetUserName()
        {
            try
            {
                var claim = _httpContextAccessor?.HttpContext?.User?.FindFirst("UserCode");
                if (!string.IsNullOrEmpty(claim?.Value))
                    return claim.Value;

                var employeeNameClaim = _httpContextAccessor?.HttpContext?.User?.FindFirst("EmployeeName");
                if (!string.IsNullOrEmpty(employeeNameClaim?.Value))
                    return employeeNameClaim.Value;

                return !string.IsNullOrEmpty(_currentUserName) ? _currentUserName : "System";
            }
            catch
            {
                return !string.IsNullOrEmpty(_currentUserName) ? _currentUserName : "System";
            }
        }

        public string GetToken()
        {
            try
            {
                var authHeader = _httpContextAccessor?.HttpContext?.Request?.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                    return authHeader.Substring("Bearer ".Length).Trim();

                return !string.IsNullOrEmpty(_currentToken) ? _currentToken : "";
            }
            catch
            {
                return !string.IsNullOrEmpty(_currentToken) ? _currentToken : "";
            }
        }

        public void SetUserContext(AuthTokenResponse authResponse)
        {
            _currentUserId = authResponse.UserCode ?? "System"; 
            _currentUserName = authResponse.EmployeeName ?? authResponse.UserCode ?? "System";
            _currentToken = authResponse.Token ?? "";
        }
    }
}