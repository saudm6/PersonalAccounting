using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Dto;

public class ReceiptLookupDto
{
    public int Id { get; init; }
    public string? ReceiptName { get; init; }
    public DateOnly ReceiptDate { get; set; }
    public IReadOnlyCollection<ReceiptDto> ReceiptItems { get; init; } = Array.Empty<ReceiptDto>();

    public class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<Receipt, ReceiptLookupDto>();
            CreateMap<ReceiptItemDto, ReceiptLookupDto>();
        }
    }

}
