using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XwearShopAPI.Model;

public partial class Cart
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int SizeId { get; set; }

    public int Count { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;

    [JsonIgnore]
    public virtual Size Size { get; set; } = null!;

    [JsonIgnore]
    public virtual User User { get; set; } = null!;
}
