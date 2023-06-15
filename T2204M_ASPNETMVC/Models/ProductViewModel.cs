using System;
using T2204M_ASPNETMVC.Entities;
namespace T2204M_ASPNETMVC.Models
{
	public class ProductViewModel
	{
		public string Name { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public Category Category { get; set; }
	}
}

