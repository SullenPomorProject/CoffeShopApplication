using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class Product
{
    public int IdProduct { get; set; }

    public int IdCategory { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? Image { get; set; }
}
