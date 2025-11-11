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

// Command for creating a new receipt with its items
public class CreateReceiptCommand : IRequest<int>
{
    // Receipt details for the command
    public string? ReceiptName { get; init; }
    public DateOnly ReceiptDate { get; init; }

    public List<ReceiptItemR> ReceiptItems { get; init; } = new();
}

// Receipt item details for the command
public class ReceiptItemR
{
    public string? ItemName { get; init; }
    public string? ItemDescription { get; init; }
    public decimal ItemPrice { get; init; }
    public int ItemQuantity { get; init; }
}

// Handler for processing the CreateReceiptCommand
public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, int>
{
    // Database context for handling the command
    private readonly IApplicationDbContext _context;

    public CreateReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    // Handle method to process the command
    public async Task<int> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {

        // Create Receipt 
        var recepitEntity = new Receipt
        {
            ReceiptName = request.ReceiptName,
            ReceiptDate = request.ReceiptDate
        };

        // Create Receipt Items and add to Receipt
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

        // Return the ID of the newly created receipt
        return recepitEntity.Id;
    }
}
