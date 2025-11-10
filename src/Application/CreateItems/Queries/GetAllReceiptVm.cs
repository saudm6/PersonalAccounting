using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.Common.Models;
using PersonalAccounting.Application.CreateItems.Dto;
using PersonalAccounting.Application.TodoLists.Queries.GetTodos;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Queries;


public class GetAllReceiptVm
{
    public IReadOnlyCollection<ReceiptLookupDto> Receipts { get; init; } = Array.Empty<ReceiptLookupDto>();

    public IReadOnlyCollection<ReceiptDto> ReceiptItems { get; init; } = Array.Empty<ReceiptDto>();
}
