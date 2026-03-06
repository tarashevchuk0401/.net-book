using FirstApi.Models;
using Microsoft.EntityFrameworkCore;

namespace FirstApi.Data
{
	public class FirstAPIContext : DbContext
	{
		public FirstAPIContext(DbContextOptions<FirstAPIContext> options):base (options) { }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		  base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<Author>().HasData(
				new Author { Id = 1, FullName = "Stephen King" },
				new Author { Id = 2, FullName = "Ernest Heminguey" }
				);

			modelBuilder.Entity<Book>().HasData(
				new Book { Id = 1, Title = "First Title", AuthorId = 1, YearPublished = 2002 },
				new Book { Id = 2, Title = "Second Title", AuthorId = 1, YearPublished = 2003 },
				new Book { Id = 3, Title = "Third Title", AuthorId = 1, YearPublished = 2004 }
				);
		}
		public DbSet<Book> Books { get; set; }

		public DbSet<Author>  Authors { get; set; }


	}
}
