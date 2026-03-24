using FirstApi.DTOs.Auth;
using FirstApi.Models;
using FirstApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly IConfiguration _config;

		public AuthController(IAuthService authService, IConfiguration configuration)
		{
			_authService = authService;
			_config = configuration;

		}

		[HttpPost("signup")]
		public async Task<ActionResult<UserDto>> SignUp(SignUpRequestDto data)
		{
			var user = await _authService.SignUp(data);

			return user;
		}

		[HttpPost("login")]
		public async Task<ActionResult<string>> LogIn(LogInRequestDto dto)
		{
			var user = await _authService.ValidateUser(dto);

			var token = _authService.GenerateJwtToken(user, _config);
			return Ok(token);
		}
	}
}
