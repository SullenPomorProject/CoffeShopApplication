using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class ShopProduct
{
    public int IdShop { get; set; }

    public int IdProduct { get; set; }

    public byte Count { get; set; }

    public virtual Product IdProductNavigation { get; set; } = null!;

    public virtual Shop IdShopNavigation { get; set; } = null!;
}
