using Coder.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Coder.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ICurrentUserService _currentUserService;

        public AuthController(
            IAuthenticationService authenticationService,
            ICurrentUserService currentUserService)
        {
            _authenticationService = authenticationService;
            _currentUserService = currentUserService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.UserCode) ||
                    string.IsNullOrWhiteSpace(request.Password))
                    return BadRequest(new { message = "UserCode and Password are required" });

                var authResponse = await _authenticationService.LoginAsync(
                    request.UserCode,
                    request.Password);

                _currentUserService.SetUserContext(authResponse);

                return Ok(new
                {
                    success = true,
                    message = "Login successful",
                    data = authResponse
                });
            }
            catch (Exception ex)
            {
                return Unauthorized(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("current-user")]
        public IActionResult GetCurrentUser()
        {
            try
            {
                return Ok(new
                {
                    userId = _currentUserService.GetUserId(),
                    userName = _currentUserService.GetUserName(),
                    message = "Current user retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class LoginRequest
    {
        public string UserCode { get; set; }
        public string Password { get; set; }
    }
}
    