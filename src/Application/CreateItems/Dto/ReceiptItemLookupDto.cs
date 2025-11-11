using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalAccounting.Application.CreateItems.Dto;
public class ReceiptItemLookupDto
{
    // Dto for receipt items
    public int Id { get; init; }
    public string? ReceiptName { get; init; }
    public DateOnly ReceiptDate { get; set; }

}
