using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class ShopInfoView2
{
    public int IdShop { get; set; }

    public int IdProduct { get; set; }

    public string ShopAddress { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public byte Count { get; set; }
}
