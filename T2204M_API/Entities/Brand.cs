using System;
using System.Collections.Generic;

namespace T2204M_API.Entities;

public partial class Brand
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Logo { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
