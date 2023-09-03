using Dashboard.Models;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Data
{
	public class ApplicationDbContext:DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
		public DbSet<Product> products { get; set; }
		public DbSet<ProductDetails> productDetails { get; set; }
		public DbSet<Invoice> invoices { get; set; }
		public DbSet<Costumer> costumers { get; set; }
		public DbSet<Cart> carts { get; set; }


	}
}
