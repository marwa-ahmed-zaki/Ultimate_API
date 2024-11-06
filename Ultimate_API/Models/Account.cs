using System;
using System.Collections.Generic;

namespace Ultimate_API.Models;

public partial class Account
{
    public string AccountCode { get; set; } = null!;

    public string? AccountName { get; set; }

    public virtual ICollection<OrderMaster> OrderMasters { get; set; } = new List<OrderMaster>();
}
