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
		private readonly IBookService _service;


		public BooksController(IBookService service)
		{
			_service = service;
		}

		[HttpGet]
		public async Task<ActionResult<List<BookDto>>> GetBooks(
			[FromQuery] int page = 1,
			[FromQuery] int pageSize = 10,
			[FromQuery] string? searchTerm = null
			)
		{

			var books = await _service.GetBooks(page, pageSize, searchTerm);
			return Ok(books);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BookDto>> GetBookById(int id)
		{
			var book = await _service.GetBookById(id);

			if (book == null) return NotFound("There are no book...");


			return Ok(book);
		}

		[HttpGet("by-year/{year}")]
		public async Task<ActionResult<List<Book>>> GetBooksByYear(int year)
		{

			var books = await _service.GetBooksByYear(year);

			if (books == null) return NotFound("No books in this year");

			return Ok(books);
		}

		[HttpPost]
		public async Task<ActionResult<Book>> AddBook(CreateBookDto newBook)
		{
			if (newBook == null)
				return BadRequest("Book is realy bad ... =( ");

			try
			{
				var book = await _service.AddBook(newBook);

				return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
			}
			catch (KeyNotFoundException ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBook(int id, CreateBookDto updatedBook)
		{


			var book = await _service.GetBookById(id);
			if (book == null)
			{
				return NotFound("There are no book...");

			}

			var newBook = await _service.UpdateBook(id, updatedBook);
			return Ok(newBook);
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteBook(int id)
		{

			await _service.DeleteBook(id);


			return Ok();


		}
	}
}
