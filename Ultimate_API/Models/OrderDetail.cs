using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ultimate_API.Models;

public partial class OrderDetail
{
    public int OrderDetailsId { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public decimal Price { get; set; }

    public int Quantity { get; set; }

    public decimal? TotalPrice { get; set; }
    [JsonIgnore]
    public virtual OrderMaster Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
