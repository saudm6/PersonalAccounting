using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalAccounting.Application.Common.Interfaces;

namespace PersonalAccounting.Application.CreateItems.Commands.CreateItem;

// Delete Request
public record DeleteReceiptById(int Id) : IRequest;

public class DeleteReceiptByIdHandler : IRequestHandler<DeleteReceiptById>
{
    private readonly IApplicationDbContext _context;

    // Database context
    public DeleteReceiptByIdHandler(IApplicationDbContext contetx)
    {
        _context = contetx;
    }

    public async Task Handle(DeleteReceiptById request, CancellationToken cancellationToken)
    {
        // Defining Entity
        var entity = await _context.Receipts
            .Where(r => r.Id == request.Id)
            .SingleOrDefaultAsync(cancellationToken);

        // Guard clause for not found entity
        Guard.Against.NotFound(request.Id, entity);

        // Removing entity
        _context.Receipts.Remove(entity);

        // Saving changes
        await _context.SaveChangesAsync(cancellationToken);

    }
}
