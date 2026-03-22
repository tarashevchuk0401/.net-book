using FirstApi.DTOs.Auth;

namespace FirstApi.Services
{
	public class AuthService :IAuthService
	{
		public async Task<string> SignUp(SignUpRequestDto dto)
		{
			return "AUth";
		}
	}
}
