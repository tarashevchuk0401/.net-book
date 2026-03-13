using FirstApi.DTOs;
using FirstApi.DTOs.Book;

public interface IBookService
{
	Task<List<BookDto>> GetBooks(int page, int pageSize, string? searchTerm);
	Task<BookDto?> GetBookById(int id);
	Task<List<BookDto>> GetBooksByYear(int year);
	Task<BookDto?> AddBook(CreateBookDto dto);
	Task<bool> UpdateBook(int id, CreateBookDto dto);
	Task<bool> DeleteBook(int id);
}