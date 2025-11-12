using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalAccounting.Application.Common.Interfaces;
using PersonalAccounting.Application.CreateItems.Dto;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Commands.CreateItem;

public class UpdateReceiptCommand : IRequest
{
    public int Id { get; init; }
    public string? ItemDescription { get; init; }
    public string? ReceiptName { get; init; }
    public DateOnly dateOnly { get; init; }
    public decimal ItemPrice { get; init; }
    public int ItemQuantity { get; init; }
    public required ICollection<ReceiptItemCmd> ReceiptItems { get; init; }
}

public class ReceiptItemCmd
{
    public string ItemName { get; set; } = default!;
    public required string ItemDescription { get; set; }
    public decimal ItemPrice { get; set; }
    public int ItemQuantity { get; set; }
}

public class UpdateReceiptCommandHandler : IRequestHandler<UpdateReceiptCommand>
{
    // Database context
    private readonly IApplicationDbContext _context;

    public UpdateReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }
    public async Task Handle(UpdateReceiptCommand request, CancellationToken cancellationToken)
    {
        // Connect to db
        var entity = await _context.Receipts
            .Include(r => r.ReceiptItems)
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        // Tell db that old values will be changed to new values
        if (entity != null)
        {
            entity.ReceiptName = request.ReceiptName;
            entity.ReceiptDate = request.dateOnly;


            // Clear existing items
            entity.ReceiptItems.Clear();
            
            // Add new items
            foreach (var item in request.ReceiptItems)
            {
                // Add each item to the receipt
                entity.AddItem(id: request.Id, itemName: item.ItemName, recepitItemEntity: );
            }
        }

        // Guard clause to check if entity is found
        Guard.Against.NotFound(request.Id, entity);

        // Save changes to the database
        await _context.SaveChangesAsync(cancellationToken);
    }
}

