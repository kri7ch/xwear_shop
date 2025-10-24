using System;
using System.Collections.Generic;

namespace XwearShopAPI.Model;

public partial class Size
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public float Size1 { get; set; }

    public float Price { get; set; }

    public virtual Product Product { get; set; } = null!;
}
