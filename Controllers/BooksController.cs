using FirstApi.Data;
using FirstApi.DTOs.Book;
using FirstApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BooksController : ControllerBase
	{
	
		private readonly FirstAPIContext _context;

		public BooksController(FirstAPIContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<List<BookDto>>> GetBooks(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string? searchTerm = null
			)
		{
			var toSkip = (page - 1) * pageSize;
			var query = _context.Books.Include(b => b.Author).AsQueryable();

			if (!string.IsNullOrWhiteSpace(searchTerm))
			{
				searchTerm = searchTerm.ToLower();
				query = query.Where(b => b.Title.ToLower().Contains(searchTerm));
			}

			var books = await query
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

			return Ok(books);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BookDto>> GetBookById(int id)
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

			if (book == null) return NotFound("There are no book...");

			return Ok(book);
		}

		[HttpGet("by-year/{year}")]
		public async Task<ActionResult<List<Book>>> GetBooksByYear(int year)
		{
		
			var book = await  _context.Books
				.Include (b => b.Author)
				.Where(book => book.YearPublished == year)
				.Select(b => new BookDto
				{
					Id = b.Id,
					Title = b.Title,
					YearPublished = b.YearPublished,
					AuthorName = b.Author!.FullName,
				})
				.ToListAsync();

			if (book == null) return NotFound("No books in this year");

			return Ok(book);
		}

		[HttpPost]
		public async Task<ActionResult<Book>> AddBook(CreateBookDto newBook)
		{
			if (newBook == null)
				return BadRequest("Book is realy bad ... =( ");

			var author = await _context.Authors.FindAsync(newBook.AuthorId);
			if(author == null)
				return BadRequest($"There is no author with id: {newBook.AuthorId}");


			var book = new Book
			{
				Title = newBook.Title,
				YearPublished = newBook.YearPublished,
				AuthorId = newBook.AuthorId
			};


			_context.Books.Add(book);
			await _context.SaveChangesAsync();

			return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, newBook);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBook(int id, Book updatedBook)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
				return NotFound("There are no book...");

			book.Title = updatedBook.Title;
			book.Author = updatedBook.Author;
			book.YearPublished = updatedBook.YearPublished;

			await _context.SaveChangesAsync();


			return NoContent();
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null)
				return NotFound("There are no book...");

			_context.Remove(book);
			await _context.SaveChangesAsync();

			return NoContent();

		}
	}
}
