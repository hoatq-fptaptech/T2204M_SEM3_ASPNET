using System;
using System.ComponentModel.DataAnnotations;
namespace T2204M_API.ViewModels
{
	public class UserRegister
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Fullname { get; set; }
		[Required]
		[MinLength(6)]
		public string Password { get; set; }
	}
}

