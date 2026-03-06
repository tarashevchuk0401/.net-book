using System.ComponentModel.DataAnnotations;

namespace FirstApi.DTOs.Book
{
	public class CreateBookDto
	{
		[Required]
		[MinLength(2)]
		public string Title { get; set; } = null!;

		[Range(1, 2100, ErrorMessage = "YearPublished must be between 1 and 2100")]
		public int YearPublished { get; set; }

		[Required]
		public int AuthorId { get; set; }
	}
}
