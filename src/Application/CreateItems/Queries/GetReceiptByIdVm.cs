using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.CreateItems.Dto;

namespace PersonalAccounting.Application.CreateItems.Queries;

// View Model for getting receipt by Id
public class GetReceiptByIdVm
{
    public IReadOnlyCollection<ReceiptLookupDto> Receipts { get; init; } = Array.Empty<ReceiptLookupDto>();
}
