using DutchTreat.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace DutchTreat.ViewModels
{
	public class OrderItemViewModel
	{
		//public Product Product { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public decimal UnitPrice { get; set; }

		[Required]
		public int OrderId { get; set; }
		[Required]
		public int ProductId { get; set; }

		//Auto-mapper gets them by walking down the FK
		public string ProductCategory { get; set; }
		public string ProductSize { get; set; }
		public string ProductTitle { get; set; }
		public string ProductArtist { get; set; }
		public string ProductArtId { get; set; }
	}
}