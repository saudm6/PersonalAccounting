using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PersonalAccounting.Application.Common.Interfaces;
using PersonalAccounting.Domain.Entities;
using PersonalAccounting.Domain.Events;

namespace PersonalAccounting.Application.CreateItems.Commands.CreateItem;

// Get all
// Get by Id

public class CreateReceiptCommand : IRequest<int>
{
    public string? ReceiptName { get; init; }
    public DateOnly ReceiptDate { get; init; }

    public List<ReceiptItemR> ReceiptItems { get; init; } = new();
}

public class ReceiptItemR
{
    public string? ItemName { get; init; }
    public string? ItemDescription { get; init; }
    public decimal ItemPrice { get; init; }
    public int ItemQuantity { get; init; }
}

public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {

        // Create Receipt 
        var recepitEntity = new Receipt
        {
            ReceiptName = request.ReceiptName,
            ReceiptDate = request.ReceiptDate
        };


        foreach (var receiptItem in request.ReceiptItems)
        {
            var recepitItemEntity = ReceiptItem.Create(receiptItem.ItemName,
                receiptItem.ItemPrice,
                receiptItem.ItemQuantity,
                receiptItem.ItemDescription, recepitEntity);


            recepitEntity.AddItem(recepitItemEntity);
        }


        _context.Receipts.Add(recepitEntity);

        await _context.SaveChangesAsync(cancellationToken);

        return recepitEntity.Id;
    }
}
