using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Dto;

public class ReceiptItemDto
{
    // Dto for receipt items
    public string? ItemName { get; init; }
    public string? ItemDescription { get; init; }
    public decimal ItemPrice { get; init; }
    public int ItemQuantity { get; init; }

    // Map ReceiptItem to ReceiptDto
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<ReceiptItem, ReceiptDto>();
        }
    }
}
