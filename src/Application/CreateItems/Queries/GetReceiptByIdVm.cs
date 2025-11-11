using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.CreateItems.Dto;

namespace PersonalAccounting.Application.CreateItems.Queries;

public class GetReceiptByIdVm
{
    // Should this be only Get? or can it be Get and Set?
    public Guid Id { get; set; }
    public IReadOnlyCollection<ReceiptLookupDto> Receipts { get; init; } = Array.Empty<ReceiptLookupDto>();
}
