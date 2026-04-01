using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FirstApi.DTOs.Auth
{
	public class LogInRequestDto
	{
		[DefaultValue("admin@example.com")]
		[Required]
		public string Email { get; set; }

		[DefaultValue("password")]
		[Required]
		public string Password { get; set; }

	}
}
