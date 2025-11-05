using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Entities;

public class StoreItems : BaseAuditableEntity
{
    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public string? ItemDescription { get; set; }

    public decimal ItemPrice { get; set; }

}
