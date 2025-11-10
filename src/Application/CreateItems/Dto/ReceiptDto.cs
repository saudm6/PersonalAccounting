using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.TodoLists.Queries.GetTodos;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Dto;

public class ReceiptDto
{
    public ReceiptDto()
    {
        Items = Array.Empty<ReceiptItemDto>();
    }

    public string? ItemName { get; set; }
    public string? ItemDescription { get; set; }
    public decimal ItemPrice { get; set; } = 0M;
    public int ItemQuantity { get; set; }
    public decimal TotalPrice { get; set; } = 0M;

    public IReadOnlyCollection<ReceiptItemDto> Items { get; init; }
    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Receipt, ReceiptDto>();
        }
    }
}
