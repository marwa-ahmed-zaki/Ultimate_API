using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ultimate_API.Models;

public partial class OrderMaster
{
    public int OrderId { get; set; }

    public string OrderNo { get; set; } = null!;

    public DateOnly OrderDate { get; set; }

    public int CustomerId { get; set; }

    public string AccountCode { get; set; } = null!;

    public string? Description { get; set; }

    public bool? IsComplete { get; set; }

    public int? EmployeeId { get; set; }

    public DateOnly? CompletedDate { get; set; }


    [JsonIgnore]
    public virtual Account AccountCodeNavigation { get; set; } = null!;
    // public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    [JsonIgnore]

    public virtual Customer Customer { get; set; } = null!;
    // public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    [JsonIgnore]

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
}
