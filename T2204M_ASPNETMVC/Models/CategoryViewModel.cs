using System.ComponentModel.DataAnnotations;
namespace T2204M_ASPNETMVC.Models
{
	public class CategoryViewModel
	{
		[Required]
		[MinLength(6,ErrorMessage = "Vui lòng nhập tối thiểu 6 ký tự")]
		[MaxLength(255)]
		[Display(Name="Tên danh mục")]
		public string Name { get; set; }
	}
}

