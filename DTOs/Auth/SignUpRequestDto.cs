using System.ComponentModel.DataAnnotations;

namespace FirstApi.DTOs.Auth
{
	public class SignUpRequestDto
	{
		[Required]
		public string FullName { get; set; }

		[Required]
		[MinLength(3)]
		public string Password { get; set; }
	}
}
