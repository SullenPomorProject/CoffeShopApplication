using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class Shop
{
    public int IdShop { get; set; }

    public string Address { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
