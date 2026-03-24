using BCrypt.Net;
using FirstApi.Data;
using FirstApi.DTOs.Auth;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Services
{
	public class AuthService :IAuthService
	{

		private FirstAPIContext _context;
		public AuthService(FirstAPIContext context) { 
			_context = context;
	
		}
		public async Task<UserDto> SignUp(SignUpRequestDto dto)
		{

			var email = dto.Email.ToLower();
			var exists = await _context.User.AnyAsync(u => u.Email == email);

			if (exists)
				throw new Exception("Email already exists");

			var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);
			var newUser = new User
			{
				FullName = dto.FullName,
				Password = hashedPassword,
				Email = email,
			};

			 _context.User.Add(newUser);
			await _context.SaveChangesAsync();

			return new UserDto
			{
				Id = newUser.Id,
				FullName = newUser.FullName,
				Email = newUser.Email
			};
		}
	}
}
