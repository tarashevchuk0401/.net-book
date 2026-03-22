using FirstApi.DTOs.Auth;
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
		public async Task<string> SignUp(SignUpRequestDto data)
		{
			return await _authService.SignUp(data);
		}
	}
}
