using System;
using System.Collections.Generic;

namespace T2204M_API.Entities;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Fullname { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int Age { get; set; }
}
