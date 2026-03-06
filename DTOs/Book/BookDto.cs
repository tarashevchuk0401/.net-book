namespace FirstApi.DTOs.Book
{
	public class BookDto
	{
		public int Id { get; set; }
		public string Title { get; set; } = null!;
		public int YearPublished { get; set; }

		public string AuthorName { get; set; } = null!;
	}
}
