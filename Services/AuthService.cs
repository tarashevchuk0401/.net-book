using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;
using FirstApi.Data;
using FirstApi.DTOs.Auth;
using FirstApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FirstApi.Services
{
	public class AuthService : IAuthService
	{

		private FirstAPIContext _context;
		public AuthService(FirstAPIContext context)
		{
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


		public async Task<UserDto> ValidateUser(LogInRequestDto dto)
		{
			var user = await _context.User.FirstOrDefaultAsync(u => u.Email == dto.Email);

			if (user == null)
			{
				throw new UnauthorizedAccessException();
			}

			bool passwordValid = BCrypt.Net.BCrypt.Verify(dto.Password, user.Password);

			if (passwordValid == false)
			{
				throw new UnauthorizedAccessException();
			}

			return new UserDto
			{
				Id = user.Id,
				Email = user.Email,
				FullName = user.FullName,
			};
		}


		public string GenerateJwtToken(UserDto user, IConfiguration config)
		{
			var jwtSettings = config.GetSection("JwtSettings");
			var key = Encoding.ASCII.GetBytes(jwtSettings["Secret"]);

			var tokenHandler = new JwtSecurityTokenHandler();
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
					new Claim(ClaimTypes.Name, user.FullName),
					new Claim(ClaimTypes.Email, user.Email)
				}
			),
				Expires = DateTime.UtcNow.AddMinutes(double.Parse(jwtSettings["ExpirationMinutes"])),
				Issuer = jwtSettings["Issuer"],
				Audience = jwtSettings["Audience"],
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(tokenDescriptor);
			return tokenHandler.WriteToken(token);
		}
	}
}
