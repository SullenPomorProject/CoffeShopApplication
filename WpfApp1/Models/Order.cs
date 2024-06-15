using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class Order
{
    public int IdOrder { get; set; }

    public int IdShop { get; set; }

    public int IdUser { get; set; }

    public DateTime OrderDatetime { get; set; }

    public string? Options { get; set; }

    public decimal? Cost { get; set; }

    public string Status { get; set; } = null!;

    public virtual Shop IdShopNavigation { get; set; } = null!;
}
