using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace XwearShopAPI.Model;

public partial class Size
{
    public int Id { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("size")]
    public float Size1 { get; set; }

    public float Price { get; set; }

    public virtual Product Product { get; set; } = null!;
}
