using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.Common.Interfaces;
using PersonalAccounting.Application.CreateItems.Dto;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.CreateItems.Commands.CreateItem;

public class UpdateReceiptCommand : IRequest
{
    public int Id { get; init; }
    public string? ReceiptName { get; init; }
    public DateOnly dateOnly { get; init; }
    public required ICollection<ReceiptItem> ReceiptItems { get; init; }
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
        // Defining Entity
        var entity = await _context.Receipts
            // Find the entity by its ID
            .FindAsync(new object[] { request.Id }, cancellationToken);

        // Guard clause to check if entity is found
        Guard.Against.NotFound(request.Id, entity);

        // Update the entity's properties
        entity.ReceiptName = request.ReceiptName;
        entity.ReceiptDate = request.dateOnly;
        entity.ReceiptItems = request.ReceiptItems;

        // Save changes to the database
        await _context.SaveChangesAsync(cancellationToken);
    }
}

