using FirstApi.DTOs.Auth;
using FirstApi.DTOs.Book;
using FirstApi.Models;

namespace FirstApi.Services
{
	public interface IAuthService
	{
		Task<UserDto> SignUp(SignUpRequestDto dto);

	}
}
