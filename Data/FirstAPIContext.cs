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

			modelBuilder.Entity<Book>().HasData(
				new Book { Id = 1, Title = "First Title", Author = "First", YearPublished = 2002 },
				new Book { Id = 2, Title = "Second Title", Author = "Second", YearPublished = 2003 },
				new Book { Id = 3, Title = "Third Title", Author = "Third", YearPublished = 2004 }
				);
		}
		public DbSet<Book> Books { get; set; }
	}
}
