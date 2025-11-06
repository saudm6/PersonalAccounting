using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Domain.Entities;


public class Receipt : BaseAuditableEntity
{
    public string? ReceiptName { get; set; }
    public DateOnly ReceiptDate { get; set; }
    public decimal ReceiptTotal { get; set; } = decimal.Zero;
    public ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();
}


public class ReceiptItem : BaseAuditableEntity
{
    public string? ItemName { get; set; }
    public string? ItemDescription { get; set; }
    public decimal ItemPrice { get; set; } = decimal.Zero;
    public int ItemQuantity { get; set; }
    public decimal TotalPrice { get; set; } = decimal.Zero;

    // References the class on Top. The Receipt class.
    public int ReceiptId { get; set; }
    [ForeignKey(nameof(ReceiptId))]
    public Receipt Receipt { get; set; } = null!;
}
