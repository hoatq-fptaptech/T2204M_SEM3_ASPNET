using System;
namespace T2204M_API.DTOs
{
	public class ProductDTO
	{
		public int id { get; set; }
		public string name { get; set; }
        public decimal price { get; set; }

        public int qty { get; set; }

        public string? description { get; set; }
    }
}

