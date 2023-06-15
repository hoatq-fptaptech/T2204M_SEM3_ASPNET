using System;
using Microsoft.EntityFrameworkCore;
namespace T2204M_ASPNETMVC.Entities
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions options): base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
	}
}

