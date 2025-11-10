using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Application.CreateItems.Dto;

public class ReceiptItemDto
{
    public string? ItemName { get; init; }
    public string? ItemDescription { get; init; }
    public decimal ItemPrice { get; init; }
    public int ItemQuantity { get; init; }
}
