using Coder.Application.DTOs.Auth;
using Coder.Application.IServices;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Coder.Application.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private string _currentToken;
        private string _currentUserId;

        public AuthenticationService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<AuthTokenResponse> LoginAsync(string userCode, string password)
        {
            try
            {
                var adminApiUrl = _configuration["AdminstratorAPI:BaseUrl"];
                var loginEndpoint = $"{adminApiUrl}";

                var loginRequest = new
                {
                    UserCode = userCode,
                    Password = password
                };

                var jsonContent = JsonSerializer.Serialize(loginRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(loginEndpoint, content);

                if (!response.IsSuccessStatusCode)
                    throw new Exception("Login failed");

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<AuthTokenResponse>(
                    jsonResponse,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                _currentToken = tokenResponse.Token;
                _currentUserId = tokenResponse.UserCode;

                return tokenResponse;
            }
            catch (Exception ex)
            {
                throw new Exception($"Authentication failed: {ex.Message}");
            }
        }

        public async Task<string> GetCurrentUserIdAsync()
        {
            return _currentUserId ?? "Unknown";
        }
    }
}

