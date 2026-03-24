using FirstApi.DTOs.Auth;
using FirstApi.DTOs.Book;
using FirstApi.Models;

namespace FirstApi.Services
{
	public interface IAuthService
	{
		Task<UserDto> SignUp(SignUpRequestDto dto);
		Task<UserDto> ValidateUser(LogInRequestDto dto);
		string GenerateJwtToken(UserDto user, IConfiguration config);

	}
}
