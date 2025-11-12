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
    public decimal ReceiptTotal { get; set; } = 0M;
    public ICollection<ReceiptItem> ReceiptItems { get; set; } = new List<ReceiptItem>();

    public void AddItem(int id, string itemName, ReceiptItem recepitItemEntity)
    {
        if (recepitItemEntity is null)
            throw new ArgumentNullException(nameof(recepitItemEntity));

        if (recepitItemEntity.ItemQuantity <= 0)
            throw new ArgumentOutOfRangeException(nameof(recepitItemEntity.ItemQuantity), "Quantity must be > 0.");

        if (recepitItemEntity.ItemPrice < 0)
            throw new ArgumentOutOfRangeException(nameof(recepitItemEntity.ItemPrice), "Price cannot be negative.");

        ReceiptItems.Add(recepitItemEntity);
        ReceiptTotal = ReceiptItems.Sum(i => i.TotalPrice);
    }
}


public class ReceiptItem : BaseAuditableEntity
{
    public string? ItemName { get; set; }
    public string? ItemDescription { get; set; }
    public decimal ItemPrice { get; set; } = 0M;
    public int ItemQuantity { get; set; }
    // This is the itemTotal
    public decimal TotalPrice { get; set; }

    // References the class on Top. The Receipt class.
    public int ReceiptId { get; set; }
    [ForeignKey(nameof(ReceiptId))]
    public Receipt Receipt { get; set; } = null!;

    public static ReceiptItem Create(string? itemName, decimal itemPrice, int itemQuantity, string? itemDescription, Receipt recepitEntity)
    {
        var recepitItemEntity = new ReceiptItem
        {
            ItemName = itemName,
            ItemDescription = itemDescription,
            ItemPrice = itemPrice,
            ItemQuantity = itemQuantity,
            Receipt = recepitEntity,
            // TotalPrice = itemPrice * Convert.ToDecimal(itemQuantity)
        };

        recepitItemEntity.TotalPrice = itemPrice * Convert.ToDecimal(itemQuantity);

        return recepitItemEntity;
    }
}
