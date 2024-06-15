using System;
using System.Collections.Generic;

namespace CoffeShopApplication.Models;

public partial class OrderFullInfoView
{
    public int IdOrder { get; set; }

    public string Address { get; set; } = null!;

    public string Customer { get; set; } = null!;

    public DateTime OrderDateTime { get; set; }

    public string? Composition { get; set; }

    public string? Options { get; set; }

    public decimal? Cost { get; set; }

    public string Status { get; set; } = null!;
}
