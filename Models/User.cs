using System.ComponentModel.DataAnnotations;

namespace FirstApi.Models
{
	public class User
	{
		public int Id { get; set; }

		public required string FullName { get; set; }

		public string Password { get; set; }

	}
}
