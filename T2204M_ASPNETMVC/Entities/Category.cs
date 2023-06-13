using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace T2204M_ASPNETMVC.Entities
{
	[Table("Categories")]
	public class Category
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string Name { get; set; }

		public ICollection<Product> Products { get; set; }
	}
}

