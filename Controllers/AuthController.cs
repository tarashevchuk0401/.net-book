using FirstApi.DTOs.Auth;
using FirstApi.Models;
using FirstApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace FirstApi.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController :  ControllerBase
	{
		private readonly IAuthService _authService;
		public AuthController(IAuthService authService) {
			_authService = authService;
		}

		[HttpPost("signup")]
		public async Task<ActionResult<UserDto>> SignUp(SignUpRequestDto data)
		{

			var user = await _authService.SignUp(data);

			return user;
		}
	}
}
