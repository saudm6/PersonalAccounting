using PersonalAccounting.Application.CreateItems.Commands.CreateItem;
using PersonalAccounting.Domain.Entities;

namespace PersonalAccounting.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Receipt> Receipts{ get; }

    DbSet<ReceiptItem> ReceiptItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
