using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class OrderedProduct
{
    public int IdOrder { get; set; }

    public int IdProduct { get; set; }

    public byte Count { get; set; }

    public virtual Order IdOrderNavigation { get; set; } = null!;

    public virtual Product IdProductNavigation { get; set; } = null!;
}
