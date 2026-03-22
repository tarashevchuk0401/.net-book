using FirstApi.DTOs.Auth;
using FirstApi.DTOs.Book;

namespace FirstApi.Services
{
	public interface IAuthService
	{
		Task<string> SignUp(SignUpRequestDto dto);

	}
}
