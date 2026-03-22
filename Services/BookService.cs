using FirstApi.Data;
using FirstApi.DTOs.Book;
using FirstApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Services
{
	public class BookService : IBookService
	{

		private readonly FirstAPIContext _context;

		public BookService(FirstAPIContext context)
		{
			_context = context;
		}

		public async Task<BookDto?> AddBook(CreateBookDto dto)
		{
			var author = await _context.Authors.FindAsync(dto.AuthorId);

			if (author == null)
				throw new KeyNotFoundException($"Author with id {dto.AuthorId} does not exist");


			var book = new Book
			{
				Title = dto.Title,
				YearPublished = dto.YearPublished,
				AuthorId = dto.AuthorId
			};

			_context.Books.Add(book);
			await _context.SaveChangesAsync();

			return new BookDto
			{
				Id = book.Id,
				Title = book.Title,
				YearPublished = book.YearPublished,
				AuthorName = author.FullName
			};
		}


		public async Task<BookDto?> GetBookById(int id)
		{
			var book = await _context.Books
			.Include(b => b.Author)
			.Where(b => b.Id == id)
			.Select(b => new BookDto
			{
				Id = b.Id,
				Title = b.Title,
				YearPublished = b.YearPublished,
				AuthorName = b.Author!.FullName,
			})
			.FirstOrDefaultAsync();

			return book;
		}

		public async Task<List<BookDto>> GetBooks(int page, int pageSize, string? searchTerm)
		{
			var toSkip = (page - 1) * pageSize;
			var query = _context.Books.Include(b => b.Author).AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				searchTerm = searchTerm.ToLower();
				query = query.Where(b => b.Title.ToLower().Contains(searchTerm));
			}

			return await query
				.OrderBy(b => b.Id)
				.Skip(toSkip)
				.Take(pageSize)
				.Select(b => new BookDto
				{
					Id = b.Id,
					Title = b.Title,
					YearPublished = b.YearPublished,
					AuthorName = b.Author!.FullName
				})
				.ToListAsync();

		}

		public async Task<List<BookDto>> GetBooksByYear(int year)
		{
			var books = await _context.Books
			.Include(b => b.Author)
			.Where(book => book.YearPublished == year)
			.Select(b => new BookDto
			{
				Id = b.Id,
				Title = b.Title,
				YearPublished = b.YearPublished,
				AuthorName = b.Author!.FullName,
			})
			.ToListAsync();

			return books;
		}

		public async Task<bool> UpdateBook(int id, CreateBookDto dto)
		{
			var book = await _context.Books.FirstOrDefaultAsync(b => b.Id == id);

			if (book == null)
			{
				return false;
			}

			var author = await _context.Authors.FindAsync(dto.AuthorId);

			if (author == null)
			{
				throw new KeyNotFoundException($"Author with id {dto.AuthorId} does not exist");
			}

			book.Title = dto.Title;
			book.YearPublished = dto.YearPublished;
			book.Author = author;

			await _context.SaveChangesAsync();

			return true;

		}

		public async Task<bool> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);

			if (book == null)
			{
				throw new KeyNotFoundException($"No books with id: {id}");
			}

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			return true;

		}
	}
}
