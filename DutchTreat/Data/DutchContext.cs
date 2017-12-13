using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
	public class DutchContext : DbContext
	{

		public DutchContext(DbContextOptions<DutchContext> options) : base(options)
		{

		}
		
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<OrderItem>()
					.HasKey(o => new { o.OrderId, o.ProductId });
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Order> Orders { get; set; }
	}
}
