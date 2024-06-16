using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace CoffeShopApplication.Models;

public partial class ShopInfoView
{
    public int IdShop { get; set; }
    public int IdProduct { get; set; }

    public string ShopAddress { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string ProductName { get; set; } = null!;

    public byte Count { get; set; }

    public ShopProduct ToShopProduct()
    {
        return new ShopProduct
        {
            IdShop = this.IdShop,
            IdProduct = this.IdProduct,
            Count = this.Count
        };
    }
}
