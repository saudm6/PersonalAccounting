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

public class CreateReceiptCommand : IRequest<int>
{
    // Receipt properties
    public string? ReceiptName { get; init; }
    public DateOnly ReceiptDate { get; init; }
    public decimal ReceiptTotal { get; init; }
    public List<ReceiptItem> ReceiptItems { get; init; } = new();

    // ReceiptItem properties
    public string? ItemName { get; init; }
    public string? ItemDescription { get; init; }
    public decimal ItemPrice { get; init; }
    public int ItemQuantity { get; init; }
    public decimal TotalPrice { get; init; }
}

public class CreateReceiptCommandHandler : IRequestHandler<CreateReceiptCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateReceiptCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public class ItemContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Receipt>()              // Refrence receipt entity
                .HasMany(r => r.ReceiptItems)           // Receipt has many ReceiptItems
                .WithOne(ri => ri.Receipt)              // Each ReceiptItem has one Receipt
                .HasForeignKey(ri => ri.ReceiptId);     // ReceiptId is the FK in ReceiptItem
        }
    }


    public async Task<int> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {

        //Create Receipt 
        var recepitEntity = new Receipt
        {
            ReceiptName = request.ReceiptName,
            ReceiptDate = request.ReceiptDate,
            ReceiptTotal = request.ReceiptTotal
        };

        //Create ReceiptItem
        var recepitItemEntity = new ReceiptItem
        {
            ItemName = request.ItemName,
            ItemDescription = request.ItemDescription,
            ItemPrice = request.ItemPrice,
            ItemQuantity = request.ItemQuantity,
            TotalPrice = request.TotalPrice,
        };

        //Add ReceiptItem to Receipts
        _context.Receipts.Add(recepitEntity);
        _context.ReceiptItems.Add(recepitItemEntity);

        return 0;
    }
}
