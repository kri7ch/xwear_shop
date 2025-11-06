using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations.Schema;

namespace XwearShopAPI.Model;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string Password { get; set; } = null!;

    public string? Name { get; set; }

    public string? Phone { get; set; }

    [NotMapped]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? CurrentPassword { get; set; }

    [NotMapped]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? NewPassword { get; set; }

    [JsonIgnore]
    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
