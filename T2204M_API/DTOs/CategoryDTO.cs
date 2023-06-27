using System;
namespace T2204M_API.DTOs
{
	public class CategoryDTO
	{
		public int id { get; set; }

		public string name { get; set; }

		public List<ProductDTO>? products { get; set; }
	}
}

