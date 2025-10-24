using System;
using System.Collections.Generic;

namespace XwearShopAPI.Model;

public partial class Model
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
